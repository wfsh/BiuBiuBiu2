using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Asset {
    public class AssetCsvManager {
        public class CsvData {
            public string filePath;
            public List<string> headers = new List<string>();
            public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
        }

        // CSV数据缓存：文件路径 → 解析后的CSV数据
        private static Dictionary<string, CsvData> csvDataCache = new Dictionary<string, CsvData>();
        private const char Separator = ',';
        private const char Quote = '"';

        // public static void Init() {
        // }

        /// <summary>
        /// 清除指定CSV文件的缓存
        /// </summary>
        public static void ClearCache(string csvFilePath) {
            if (csvDataCache.ContainsKey(csvFilePath)) {
                csvDataCache.Remove(csvFilePath);
                Debug.Log($"已清除CSV缓存: {csvFilePath}");
            }
        }

        /// <summary>
        /// 清除所有CSV缓存
        /// </summary>
        public static void ClearAllCache() {
            csvDataCache.Clear();
            Debug.Log("已清除所有CSV缓存");
        }

        /// <summary>
        /// 根据文件路径和查询条件获取CSV行数据
        /// </summary>
        public static void GetCsvRow(string csvFilePath, Dictionary<string, object> queryConditions,
            Action<Dictionary<string, string>> onRowFound) {
            if (string.IsNullOrEmpty(csvFilePath)) {
                Debug.LogError("CSV文件路径为空");
                onRowFound?.Invoke(null);
                return;
            }
            if (queryConditions == null || queryConditions.Count == 0) {
                Debug.LogError("查询条件为空");
                onRowFound?.Invoke(null);
                return;
            }
            // 尝试从缓存获取CSV数据
            if (!csvDataCache.TryGetValue(csvFilePath, out var csvData)) {
                LoadAndParseCsv(csvFilePath, data => {
                    csvDataCache[csvFilePath] = data;
                    FindRow(data, queryConditions, onRowFound);
                });
            } else {
                FindRow(csvData, queryConditions, onRowFound);
            }
        }
        
        public static void GetAbilityABAllCsvRows(string sign, Action<List<Dictionary<string, string>>> onRowsGot) {
            var url = $"{AssetURL.AbilityCSV}/AB/{sign}";
            GetAllCsvRows(url, onRowsGot);
        }
        
        public static void GetAbilityAEAllCsvRows(string sign, Action<List<Dictionary<string, string>>> onRowsGot) {
            var url = $"{AssetURL.AbilityCSV}/AE/{sign}";
            GetAllCsvRows(url, onRowsGot);
        }
        

        /// 全量获取CSV的所有行数据（供业务类一次性解析实例）
        public static void GetAllCsvRows(string csvFilePath, Action<List<Dictionary<string, string>>> onRowsGot) {
            if (string.IsNullOrEmpty(csvFilePath)) {
                Debug.LogError("CSV文件路径为空");
                onRowsGot?.Invoke(null);
                return;
            }
            // 先查文件缓存，避免重复加载解析CSV - 业务层已经做了缓存。这边不在做处理
            // if (csvDataCache.TryGetValue(csvFilePath, out var csvData)) {
            //     onRowsGot?.Invoke(csvData.rows);
            //     return;
            // }
            // 缓存未命中，加载并解析CSV（仅一次）
            LoadAndParseCsv(csvFilePath, data => {
                if (data == null || data.rows.Count == 0) {
                    Debug.LogError($"CSV全量加载失败：{csvFilePath}");
                    onRowsGot?.Invoke(null);
                    return;
                }
                // csvDataCache[csvFilePath] = data; // 业务层已经做了缓存。这边不在做处理
                onRowsGot?.Invoke(data.rows);
            });
        }

        // /// <summary>
        // /// 通用获取CSV行值
        // /// </summary>
        // public static string GetCsvRowValue(Dictionary<string, string> csvRow, string key) {
        //     if (csvRow != null && csvRow.ContainsKey(key)) {
        //         return csvRow[key];
        //     }
        //     return string.Empty;
        // }
        /// <summary>
        /// 从 CSV 获取单个字段值
        /// </summary>
        public static string GetCsvRowValue(Dictionary<string, string> csvRow, string key) {
            return csvRow.TryGetValue(key, out var value) ? value : null;
        }

        // 数组解析方式
        /// <summary>
        /// 一维数组 1|2
        /// 二维数组 1|2&3|4|5
        /// Vector3 1;2;3
        /// 逗号(,) 不能使用
        /// </summary>
        public static T ParseValue<T>(string raw) {
            if (string.IsNullOrEmpty(raw)) return default;
            var type = typeof(T);

            // --- 一维或二维数组 ---
            if (type.IsArray) {
                var elementType = type.GetElementType();

                // 二维数组
                if (elementType.IsArray) {
                    var innerType = elementType.GetElementType();
                    // & 分隔外层
                    var blocks = raw.Split('&');
                    var outerArray = Array.CreateInstance(elementType, blocks.Length);
                    for (int i = 0; i < blocks.Length; i++) {
                        var block = blocks[i].Trim();
                        // | 分隔内层
                        var innerParts = block.Split('|');
                        var innerArray = Array.CreateInstance(innerType, innerParts.Length);
                        for (int j = 0; j < innerParts.Length; j++) {
                            innerArray.SetValue(ConvertSingle(innerParts[j], innerType), j);
                        }
                        outerArray.SetValue(innerArray, i);
                    }
                    return (T)(object)outerArray;
                } else {
                    // 一维数组
                    var parts = raw.Split('|');
                    var arr = Array.CreateInstance(elementType, parts.Length);
                    for (int i = 0; i < parts.Length; i++) {
                        arr.SetValue(ConvertSingle(parts[i], elementType), i);
                    }
                    return (T)(object)arr;
                }
            }
            // --- 单值 ---
            return (T)ConvertSingle(raw, type);
        }

        private static object ConvertSingle(string raw, Type targetType) {
            raw = raw.Trim();
            try {
                if (targetType == typeof(string)) return raw;
                if (targetType == typeof(int)) return int.Parse(raw);
                if (targetType == typeof(short)) return short.Parse(raw);
                if (targetType == typeof(ushort)) return ushort.Parse(raw);
                if (targetType == typeof(byte)) return byte.Parse(raw);
                if (targetType == typeof(sbyte)) return sbyte.Parse(raw);
                if (targetType == typeof(float)) return float.Parse(raw);
                if (targetType == typeof(bool)) {
                    if (raw == "1" || raw == "0") {
                        return raw == "1";
                    } else {
                        return bool.Parse(raw);
                    }
                };
                if (targetType == typeof(Vector3)) return ParseVector3(raw);
                Debug.LogError("AssetCsvManager 不支持的类型转换: " + targetType);
            } catch (Exception e) {
                Debug.LogError($"AssetCsvManager 类型转换错误: {targetType} from '{raw}'\n{e}");
                return null;
            }
            return null;
        }
        
        /// <summary>
        /// string to Vector3, 格式: "x;y;z"
        /// </summary>
        public static Vector3 ParseVector3(string raw) {
            var parts = raw.Split(';');
            return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
        }

        /// <summary>
        /// string to Vector3
        /// </summary>
        public static Vector3 GetCsvVector3(Dictionary<string, string> csvRow, string key) {
            Vector3 csvVector = Vector3.zero;
            if (!string.IsNullOrEmpty(GetCsvRowValue(csvRow, key))) {
                var parts = GetCsvRowValue(csvRow, key).Split(',');
                if (parts.Length == 3 &&
                    float.TryParse(parts[0], out var effectPosX) &&
                    float.TryParse(parts[1], out var effectPosY) &&
                    float.TryParse(parts[2], out var effectPosZ)) {
                    csvVector = new Vector3(effectPosX, effectPosY, effectPosZ);
                }
            }
            return csvVector;
        }

        // 查找匹配的行
        private static void FindRow(CsvData csvData, Dictionary<string, object> queryConditions,
            Action<Dictionary<string, string>> callBack) {
#if UNITY_EDITOR
            const int RowThreshold = 100;
            if (csvData.rows.Count > RowThreshold) {
                Debug.LogError($"CSV文件 {csvData.filePath} 行数 {csvData.rows.Count} 超过阈值 {RowThreshold}，可能需要优化查找方式。");
            }
#endif
            foreach (var row in csvData.rows) {
                bool isMatch = true;
                foreach (var kv in queryConditions) {
                    if (!row.TryGetValue(kv.Key, out var rowValue)) {
                        isMatch = false;
                        break;
                    }
                    if (!ValuesAreEqual(rowValue, kv.Value)) {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch) {
                    callBack?.Invoke(row);
                    return;
                }
            }
            Debug.LogError($"CSV文件 {csvData.filePath} 未找到匹配条件: {GetConditionsString(queryConditions)}");
            callBack?.Invoke(null);
        }

        /// <summary>
        /// 加载并解析CSV文件
        /// </summary>
        private static void LoadAndParseCsv(string csvFile, Action<CsvData> onLoadEnd) {
            AssetManager.LoadCSV(csvFile, txtAsset => {
                var data = ParseCsvContent(txtAsset.text);
                if (data == null || data.headers.Count == 0) {
                    Debug.LogError($"CSV文件 {csvFile} 解析失败或没有表头");
                }
                data.filePath = csvFile;
                onLoadEnd?.Invoke(data);
            });
        }

        /// <summary>
        /// 解析CSV内容
        /// </summary>
        private static CsvData ParseCsvContent(string csvContent) {
            var csvData = new CsvData();
            var lines = SplitCsvLines(csvContent);
            if (lines.Count == 0) {
                Debug.LogError("CSV内容没有任何行");
                return csvData;
            }
            // 表头
            csvData.headers = ParseCsvLine(lines[0]);
            // 数据行
            for (int i = 1; i < lines.Count; i++) {
                var line = lines[i];
                try {
                    var fields = ParseCsvLine(line);
                    var rowData = new Dictionary<string, string>();
                    for (int j = 0; j < csvData.headers.Count; j++) {
                        var header = csvData.headers[j];
                        var value = j < fields.Count ? fields[j] : "";
                        rowData[header] = value;
                    }
                    csvData.rows.Add(rowData);
                } catch (Exception e) {
                    Debug.LogError($"CSV 解析错误 行 {i + 1}: {e}\n内容: {line}");
                }
            }
            return csvData;
        }

        /// <summary>
        /// 按换行符分割CSV行，支持引号内换行
        /// </summary>
        private static List<string> SplitCsvLines(string csvContent) {
            if (!csvContent.Contains(Quote)) {
                return new List<string>(
                    csvContent.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                );
            }
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            var inQuotes = false;
            for (int i = 0; i < csvContent.Length; i++) {
                char c = csvContent[i];
                if (c == Quote) {
                    inQuotes = !inQuotes;
                    currentLine.Append(c);
                } else if ((c == '\n' || c == '\r') && !inQuotes) {
                    if (currentLine.Length > 0) {
                        lines.Add(currentLine.ToString());
                        currentLine.Clear();
                    }
                    if (c == '\r' && i + 1 < csvContent.Length && csvContent[i + 1] == '\n') {
                        i++; // 跳过 \r\n 中的 \n
                    }
                } else {
                    currentLine.Append(c);
                }
            }
            if (currentLine.Length > 0) {
                lines.Add(currentLine.ToString());
            }
            return lines;
        }

        /// <summary>
        /// 解析一行
        /// </summary>
        private static List<string> ParseCsvLine(string line) {
            // 快速路径: 没有引号，直接 Split
            if (!line.Contains(Quote)) {
                return new List<string>(line.Split(Separator));
            }
            var fields = new List<string>();
            var currentField = new StringBuilder();
            var inQuotes = false;
            for (int i = 0; i < line.Length; i++) {
                var c = line[i];
                if (c == Quote) {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == Quote) {
                        // 转义的双引号 ""
                        currentField.Append(Quote);
                        i++;
                    } else {
                        inQuotes = !inQuotes;
                    }
                } else if (c == Separator && !inQuotes) {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                } else {
                    currentField.Append(c);
                }
            }
            fields.Add(currentField.ToString());
            return fields;
        }

        /// <summary>
        /// 比较值是否相等
        /// </summary>
        private static bool ValuesAreEqual(object value1, object value2) {
            if (value1 == null && value2 == null) return true;
            if (value1 == null || value2 == null) return false;
            var s1 = value1.ToString();
            var s2 = value2.ToString();

            // 尝试数值比较
            if (double.TryParse(s1, out var n1) && double.TryParse(s2, out var n2)) {
                return Math.Abs(n1 - n2) < 0.0001;
            }
            return string.Equals(s1, s2, StringComparison.Ordinal);
        }

        /// <summary>日志用：把查询条件转为字符串</summary>
        private static string GetConditionsString(Dictionary<string, object> conditions) {
            var sb = new StringBuilder();
            sb.Append("{ ");
            foreach (var kv in conditions) {
                sb.Append($"{kv.Key} = {kv.Value}, ");
            }
            if (sb.Length > 2) sb.Length -= 2;
            sb.Append(" }");
            return sb.ToString();
        }
    }
}