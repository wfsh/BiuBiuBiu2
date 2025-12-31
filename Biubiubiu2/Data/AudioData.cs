using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class AudioData {
        public enum AudioTypeEnum : byte {
            Undefined = 0,
            Audio = 1,
            Voice = 2,
        }

        public static bool GetWwiseEventAudioById(ushort id, out string eventName) {
            eventName = "";
            if (WwiseAudioSet.HasWwiseAudioById(id)) {
                return false;
            }
            eventName = WwiseAudioSet.GetWwiseAudioById(id).WwiseEvent;
            return true;
        }

        public static string GetWwiseEventAudioById(ushort id) {
            if (WwiseAudioSet.HasWwiseAudioById(id)) {
                return "";
            }
            return WwiseAudioSet.GetWwiseAudioById(id).WwiseEvent;
        }
    }
}