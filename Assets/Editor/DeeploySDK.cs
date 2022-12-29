using System.Collections.Generic;
using UnityEngine;
using System;

public partial class DeeploySDK
{
    public partial class GameAnalyticsSDK
    {
        public partial class GameAnalytics : MonoBehaviour
        {
            public enum GAProgressionStatus
            {
                Undefined = 0,
                Start = 1,
                Complete = 2,
                Fail = 3
            }
            public enum GAResourceFlowType
            {
                Undefined = 0,
                Source = 1,
                Sink = 2

            }
            public enum GAErrorSeverity
            {
                Undefined = 0,
                Debug = 1,
                Info = 2,
                Warning = 3,
                Error = 4,
                Critical = 5
            }
            public enum GAAdAction
            {
                Undefined = 0,
                Clicked = 1,
                Show = 2,
                FailedShow = 3,
                RewardReceived = 4,
                Request = 5,
                Loaded = 6
            }
            public enum GAAdType
            {
                Undefined = 0,
                Video = 1,
                RewardedVideo = 2,
                Playable = 3,
                Interstitial = 4,
                OfferWall = 5,
                Banner = 6
            }
            public enum GAAdError
            {
                Undefined = 0,
                Unknown = 1,
                Offline = 2,
                NoFill = 3,
                InternalError = 4,
                InvalidRequest = 5,
                UnableToPrecache = 6
            }
            public static void Initialize() { }
            public static void NewBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType) { }
            public static void NewBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewDesignEvent(string eventName) { }
            public static void NewDesignEvent(string eventName, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewDesignEvent(string eventName, float eventValue) { }
            public static void NewDesignEvent(string eventName, float eventValue, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, int score) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, int score, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, int score, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score) { }
            public static void NewProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId) { }
            public static void NewResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewErrorEvent(GAErrorSeverity severity, string message) { }
            public static void NewErrorEvent(GAErrorSeverity severity, string message, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewAdEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, long duration) { }
            public static void NewAdEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, long duration, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewAdEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, GAAdError noAdReason) { }
            public static void NewAdEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, GAAdError noAdReason, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void NewAdEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement) { }
            public static void NewAdEvent(GAAdAction adAction, GAAdType adType, string adSdkName, string adPlacement, IDictionary<string, object> customFields, bool mergeFields = false) { }
            public static void SetCustomId(string userId) { }
            public static void SetEnabledManualSessionHandling(bool enabled) { }
            public static void SetEnabledEventSubmission(bool enabled) { }
            public static void StartSession() { }
            public static void EndSession() { }
            public static void SetCustomDimension01(string customDimension) { }
            public static void SetCustomDimension02(string customDimension) { }
            public static void SetCustomDimension03(string customDimension) { }
            public static void SetGlobalCustomEventFields(IDictionary<string, object> customFields) { }
            public static event Action OnRemoteConfigsUpdatedEvent;
            public static void RemoteConfigsUpdated() { }
            public static string GetRemoteConfigsValueAsString(string key) { return null; }
            public static string GetRemoteConfigsValueAsString(string key, string defaultValue) { return null; }
            public static bool IsRemoteConfigsReady() { return false; }
            public static string GetRemoteConfigsContentAsString() { return null; }
            public static string GetABTestingId() { return null; }
            public static string GetABTestingVariantId() { return null; }
            public static void StartTimer(string key) { }
            public static void PauseTimer(string key) { }
            public static void ResumeTimer(string key) { }
            public static long StopTimer(string key) { return 0; }
            public static void SetBuildAllPlatforms(string build) { }
        }
    }
}