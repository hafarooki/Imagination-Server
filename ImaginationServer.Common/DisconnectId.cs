namespace ImaginationServer.Common
{
    public enum DisconnectId : uint
    {
        UnknownServerError = 0x00,
        DuplicateLogin = 0x04,
        ServerShutdown = 0x05,
        ServerUnableToLoadMap = 0x06,
        InvalidSessionKey = 0x07,
        AccountNotInPendingList = 0x08,
        CharacterNotFound = 0x09,
        CharacterCorruption = 0x0a,
        Kick = 0x0b,
        FreeTrialExpire = 0x0d,
        PlaytimeScheduleDone = 0x0e
    }
}