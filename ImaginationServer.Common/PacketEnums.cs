namespace ImaginationServer.Common
{
    public class PacketEnums
    {
        public enum RemoteConnection : ushort
        {
            Server = 0x00,
            Auth = 0x01,
            Chat = 0x02,
            Unknown = 0x03,
            World = 0x04,
            Client = 0x05
        }

        // IDs for all servers
        public enum ServerPacketId : uint
        {
            MsgServerVersionConfirm = 0x00,
            MsgServerDisconnectNotify = 0x01,
            MsgServerGeneralNotify = 0x02,
        }

        public enum ChatPacketId : uint
        {
            MsgChatLoginSessionNotify = 0x00,
            MsgChatGeneralChatMessage = 0x01,
            MsgChatPrivateChatMessage = 0x02,
            MsgChatUserChannelChatMessage = 0x03,
            MsgChatWorldDisconnectRequest = 0x04,
            MsgChatWorldProximityResponse = 0x05,
            MsgChatWorldParcelResponse = 0x06,
            MsgChatAddFriendRequest = 0x07,
            MsgChatAddFriendResponse = 0x08,
            MsgChatRemoveFriend = 0x09,
            MsgChatGetFriendsList = 0x0a,
            MsgChatAddIgnore = 0x0b,
            MsgChatRemoveIgnore = 0x0c,
            MsgChatGetIgnoreList = 0x0d,
            MsgChatTeamMissedInviteCheck = 0x0e,
            MsgChatTeamInvite = 0x0f,
            MsgChatTeamInviteResponse = 0x10,
            MsgChatTeamKick = 0x11,
            MsgChatTeamLeave = 0x12,
            MsgChatTeamSetLoot = 0x13,
            MsgChatTeamSetLeader = 0x14,
            MsgChatTeamGetStatus = 0x15,
            MsgChatGuildCreate = 0x16,
            MsgChatGuildInvite = 0x17,
            MsgChatGuildInviteResponse = 0x18,
            MsgChatGuildLeave = 0x19,
            MsgChatGuildKick = 0x1a,
            MsgChatGuildGetStatus = 0x1b,
            MsgChatGuildGetAll = 0x1c,
            MsgChatShowAll = 0x1d,
            MsgChatBlueprintModerated = 0x1e,
            MsgChatBlueprintModelReady = 0x1f,
            MsgChatPropertyReadyForApproval = 0x20,
            MsgChatPropertyModerationChanged = 0x21,
            MsgChatPropertyBuildmodeChanged = 0x22,
            MsgChatPropertyBuildmodeChangedReport = 0x23,
            MsgChatMail = 0x24,
            MsgChatWorldInstanceLocationRequest = 0x25,
            MsgChatReputationUpdate = 0x26,
            MsgChatSendCannedText = 0x27,
            MsgChatGmlevelUpdate = 0x28,
            MsgChatCharacterNameChangeRequest = 0x29,
            MsgChatCsrRequest = 0x2a,
            MsgChatCsrReply = 0x2b,
            MsgChatGmKick = 0x2c,
            MsgChatGmAnnounce = 0x2d,
            MsgChatGmMute = 0x2e,
            MsgChatActivityUpdate = 0x2f,
            MsgChatWorldRoutePacket = 0x30,
            MsgChatGetZonePopulations = 0x31,
            MsgChatRequestMinimumChatMode = 0x32,
            MsgChatRequestMinimumChatModePrivate = 0x33,
            MsgChatMatchRequest = 0x34,
            MsgChatUgcmanifestReportMissingFile = 0x35,
            MsgChatUgcmanifestReportDoneFile = 0x36,
            MsgChatUgcmanifestReportDoneBlueprint = 0x37,
            MsgChatUgccRequest = 0x38,
            MsgChatWho = 0x39,
            MsgChatWorldPlayersPetModeratedAcknowledge = 0x3a,
            MsgChatAchievementNotify = 0x3b,
            MsgChatGmClosePrivateChatWindow = 0x3c,
            MsgChatUnexpectedDisconnect = 0x3d,
            MsgChatPlayerReady = 0x3e,
            MsgChatGetDonationTotal = 0x3f,
            MsgChatUpdateDonation = 0x40,
            MsgChatPrgCsrCommand = 0x41,
            MsgChatHeartbeatRequestFromWorld = 0x42,
            MsgChatUpdateFreeTrialStatus = 0x43
        }

        public enum ClientAuthPacketId : uint
        {
            MsgAuthLoginRequest = 0x00,
            MsgAuthLogoutRequest = 0x01,
            MsgAuthCreateNewAccountRequest = 0x02,
            MsgAuthLegointerfaceAuthResponse = 0x03,
            MsgAuthSessionkeyReceivedConfirm = 0x04,
            MsgAuthRuntimeConfig = 0x05
        }

        public enum ClientWorldPacketId : uint
        {
            /// <summary>
            /// Session info
            /// </summary>
            MsgWorldClientValidation = 0x01,
            MsgWorldClientCharacterListRequest = 0x02,
            MsgWorldClientCharacterCreateRequest = 0x03,
            /// <summary>
            /// Character selected
            /// </summary>
            MsgWorldClientLoginRequest = 0x04,
            MsgWorldClientGameMsg = 0x05,
            MsgWorldClientCharacterDeleteRequest = 0x06,
            MsgWorldClientCharacterRenameRequest = 0x07,
            MsgWorldClientHappyFlowerModeNotify = 0x08,
            MsgWorldClientSlashReloadMap = 0x09,
            MsgWorldClientSlashPushMapRequest = 0x0a,
            MsgWorldClientSlashPushMap = 0x0b,
            MsgWorldClientSlashPullMap = 0x0c,
            MsgWorldClientLockMapRequest = 0x0d,
            MsgWorldClientGeneralChatMessage = 0x0e,
            MsgWorldClientHttpMonitorInfoRequest = 0x0f,
            MsgWorldClientSlashDebugScripts = 0x10,
            MsgWorldClientModelsClear = 0x11,
            MsgWorldClientExhibitInsertModel = 0x12,
            MsgWorldClientLevelLoadComplete = 0x13,
            MsgWorldClientTmpGuildCreate = 0x14,
            MsgWorldClientRoutePacket = 0x15,
            MsgWorldClientPositionUpdate = 0x16,
            MsgWorldClientMail = 0x17,
            MsgWorldClientWordCheck = 0x18,
            MsgWorldClientStringCheck = 0x19,
            MsgWorldClientGetPlayersInZone = 0x1a,
            MsgWorldClientRequestUgcManifestInfo = 0x1b,
            MsgWorldClientBlueprintGetAllDataRequest = 0x1c,
            MsgWorldClientCancelMapQueue = 0x1d,
            MsgWorldClientHandleFunness = 0x1e,
            MsgWorldClientFakePrgCsrMessage = 0x1f,
            MsgWorldClientRequestFreeTrialRefresh = 0x20,
            MsgWorldClientGmSetFreeTrialStatus = 0x21,
            MsgWorldTop5IssuesRequest = 0x22,
            MsgWorldUgcDownloadFailedT = 0x23,
            MsgWorldUgcDownloadFailed = 0x78
        }

        public enum WorldServerPacketId : uint
        {
            MsgClientLoginResponse = 0x00,
            MsgClientLogoutResponse = 0x01,
            MsgClientLoadStaticZone = 0x02,
            MsgClientCreateObject = 0x03,
            MsgClientCreateCharacter = 0x04,
            MsgClientCreateCharacterExtended = 0x05,
            MsgClientCharacterListResponse = 0x06,
            MsgClientCharacterCreateResponse = 0x07,
            MsgClientCharacterRenameResponse = 0x08,
            MsgClientChatConnectResponse = 0x09,
            MsgClientAuthAccountCreateResponse = 0x0a,
            MsgClientDeleteCharacterResponse = 0x0b,
            MsgClientGameMsg = 0x0c,
            MsgClientConnectChat = 0x0d,
            MsgClientTransferToWorld = 0x0e,
            MsgClientImpendingReloadNotify = 0x0f,
            MsgClientMakeGmResponse = 0x10,
            MsgClientHttpMonitorInfoResponse = 0x11,
            MsgClientSlashPushMapResponse = 0x12, // Push map
            MsgClientSlashPullMapResponse = 0x13, // Pull map
            MsgClientSlashLockMapResponse = 0x14, // Lock map
            MsgClientBlueprintSaveResponse = 0x15,
            MsgClientBlueprintLupSaveResponse = 0x16,
            MsgClientBlueprintLoadResponseItemid = 0x17,
            MsgClientBlueprintGetAllDataResponse = 0x18,
            MsgClientModelInstantiateResponse = 0x19,
            MsgClientDebugOutput = 0x1a,
            MsgClientAddFriendRequest = 0x1b,
            MsgClientAddFriendResponse = 0x1c,
            MsgClientRemoveFriendResponse = 0x1d,
            MsgClientGetFriendsListResponse = 0x1e,
            MsgClientUpdateFriendNotify = 0x1f,
            MsgClientAddIgnoreResponse = 0x20,
            MsgClientRemoveIgnoreResponse = 0x21,
            MsgClientGetIgnoreListResponse = 0x22,
            MsgClientTeamInvite = 0x23,
            MsgClientTeamInviteInitialResponse = 0x24,
            MsgClientGuildCreateResponse = 0x25,
            MsgClientGuildGetStatusResponse = 0x26,
            MsgClientGuildInvite = 0x27,
            MsgClientGuildInviteInitialResponse = 0x28,
            MsgClientGuildInviteFinalResponse = 0x29,
            MsgClientGuildInviteConfirm = 0x2a,
            MsgClientGuildAddPlayer = 0x2b,
            MsgClientGuildRemovePlayer = 0x2c,
            MsgClientGuildLoginLogout = 0x2d,
            MsgClientGuildRankChange = 0x2e,
            MsgClientGuildData = 0x2f,
            MsgClientGuildStatus = 0x30,
            MsgClientMail = 0x31,
            MsgClientDbProxyResult = 0x32,
            MsgClientShowAllResponse = 0x33,
            MsgClientWhoResponse = 0x34,
            MsgClientSendCannedText = 0x35,
            MsgClientUpdateCharacterName = 0x36,
            MsgClientSetNetworkSimulator = 0x37,
            MsgClientInvalidChatMessage = 0x38,
            MsgClientMinimumChatModeResponse = 0x39,
            MsgClientMinimumChatModeResponsePrivate = 0x3a,
            MsgClientChatModerationString = 0x3b,
            MsgClientUgcManifestResponse = 0x3c,
            MsgClientInLoginQueue = 0x3d,
            MsgClientServerStates = 0x3e,
            MsgClientGmCloseTargetChatWindow = 0x3f,
            MsgClientGeneralTextForLocalization = 0x40,
            MsgClientUpdateFreeTrialStatus = 0x41
        }
    }
}
