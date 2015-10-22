using ImaginationServer.Common.Data;

namespace ImaginationServer.Common
{
    /// <summary>
    ///     Utility class, used for managing the Redis database.
    /// </summary>
    public class DbUtils
    {
        #region Character Stuff

        /// <summary>
        ///     Gets the key of the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="notRaw">Is this not raw (not the actual raw object id from the client/RM/whatever?)</param>
        /// <returns>The key where the username assigned to that ID should be stored.</returns>
        public static string GetIdKey(long id, bool notRaw = false)
        {
            return $"characters:IDMAP:{id - (notRaw ? 0 : 1152921504606846994)}";
        }

        /// <summary>
        ///     Gets the key of the specified character.
        /// </summary>
        /// <param name="username">The character's name.</param>
        /// <returns>The key where the character's data is stored in the database.</returns>
        public static string GetCharacterKey(string username)
        {
            return $"characters:{username.ToLower()}";
        }

        /// <summary>
        ///     Does a character of this ID exist?
        /// </summary>
        /// <param name="id">The ID of the supposed character.</param>
        /// <param name="notRaw">Is this not raw?</param>
        /// <returns>
        ///     Whether or not there is a character of that ID, and if there is, whether or not that character still actually
        ///     exists.
        /// </returns>
        public static bool CharacterExists(long id, bool notRaw = false)
        {
            return LuServer.CurrentServer.CacheClient.Exists(GetIdKey(id, notRaw)) && // Check if the key exists
                   CharacterExists(GetCharacterName(id));
                // Check if a character of the username assigned to the key exists
        }

        /// <summary>
        ///     Does a character of this username exist?
        /// </summary>
        /// <param name="username">The username of the supposed character.</param>
        /// <returns>Whether or not this character actually exists.</returns>
        public static bool CharacterExists(string username)
        {
            return LuServer.CurrentServer.CacheClient.Exists(GetCharacterKey(username));
        }

        /// <summary>
        ///     Adds a character to the database.
        /// </summary>
        /// <param name="character">The character to add.</param>
        public static void AddCharacter(Character character)
        {
            LuServer.CurrentServer.CacheClient.Add(GetCharacterKey(character.Minifig.Name), character);
        }

        /// <summary>
        ///     Update the data for an existing character.
        /// </summary>
        /// <param name="character">The character to update.</param>
        public static void UpdateCharacter(Character character)
        {
            LuServer.CurrentServer.CacheClient.Remove(GetCharacterKey(character.Minifig.Name));
                // First remove it (doesn't seem to be a Set method, so we have to remove first?)
            LuServer.CurrentServer.CacheClient.Add(GetCharacterKey(character.Minifig.Name), character); // Then add it.
        }

        /// <summary>
        ///     Removes a character from the database, and its owner account.
        /// </summary>
        /// <param name="character">The character to delete.</param>
        public static void DeleteCharacter(Character character)
        {
            LuServer.CurrentServer.CacheClient.Remove(GetCharacterKey(character.Minifig.Name));
            if (character.Owner == null || !AccountExists(character.Owner))
                return; // No need to do anything else if there is no valid account assigned to the specified character.
            var account = GetAccount(character.Owner);
            account.Characters.Remove(character.Minifig.Name);
            UpdateAccount(account);
        }


        /// <summary>
        ///     Gets the name assigned to the specified ID.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <param name="notRaw">Is this not raw?</param>
        /// <returns>Whatever name is assigned to the specified ID.</returns>
        public static string GetCharacterName(long id, bool notRaw = false)
        {
            return LuServer.CurrentServer.CacheClient.Database.StringGet(GetIdKey(id, notRaw));
        }

        /// <summary>
        ///     Gets a character from their ID.
        /// </summary>
        /// <param name="id">The character ID.</param>
        /// <param name="notRaw">Is this not raw?</param>
        /// <returns>The character</returns>
        public static Character GetCharacter(long id, bool notRaw = false)
        {
            return GetCharacter(GetCharacterName(id, notRaw));
        }

        /// <summary>
        ///     Gets a character from their name.
        /// </summary>
        /// <param name="name">The character's name.</param>
        /// <returns>The character</returns>
        public static Character GetCharacter(string name)
        {
            return LuServer.CurrentServer.CacheClient.Get<Character>(GetCharacterKey(name.ToLower()));
        }

        #endregion

        #region Account Stuff

        /// <summary>
        ///     Gets the key of an account with the specified username.
        /// </summary>
        /// <param name="username">The username to look for.</param>
        /// <returns>A key that would store an account of said username.</returns>
        public static string GetAccountKey(string username)
        {
            return $"accounts:{username.ToLower()}";
        }

        /// <summary>
        ///     Does an account of this username exist?
        /// </summary>
        /// <param name="username">The username of the supposed account.</param>
        /// <returns>Whether an account of the specified username exists.</returns>
        public static bool AccountExists(string username)
        {
            return LuServer.CurrentServer.CacheClient.Exists(GetAccountKey(username));
        }

        /// <summary>
        ///     Creates an account with specified username and password.
        /// </summary>
        /// <param name="username">The username of the account.</param>
        /// <param name="password">The password of the account.</param>
        /// <returns>The account that was created.</returns>
        public static Account CreateAccount(string username, string password)
        {
            var account = Account.Create(username, password);
            LuServer.CurrentServer.CacheClient.Add(GetAccountKey(username), account);
            return account;
        }

        /// <summary>
        ///     Updates the data of an existing account. (Maybe you want to change the password, or add/remove users, or ban/unban
        ///     the account?)
        /// </summary>
        /// <param name="account">The account to update.</param>
        public static void UpdateAccount(Account account)
        {
            LuServer.CurrentServer.CacheClient.Remove(GetAccountKey(account.Username));
                // First remove, there doesn't seem to be a set method in CacheClient?
            LuServer.CurrentServer.CacheClient.Add(GetAccountKey(account.Username), account);
                // Add the account back in.
        }

        /// <summary>
        ///     Deletes the specified account.
        /// </summary>
        /// <param name="account">The account to delete.</param>
        public static void DeleteAccount(Account account)
        {
            // Delete its characters
            foreach (var character in account.Characters)
            {
                if (!CharacterExists(character)) continue; // Maybe this character was deleted by error, or otherwise?
                DeleteCharacter(GetCharacter(character));
            }

            LuServer.CurrentServer.CacheClient.Remove(GetAccountKey(account.Username));
        }

        /// <summary>
        ///     Retrieves the data of account with the specified username.
        /// </summary>
        /// <param name="username">The username of the account.</param>
        /// <returns></returns>
        public static Account GetAccount(string username)
        {
            return LuServer.CurrentServer.CacheClient.Get<Account>(GetAccountKey(username));
        }

        #endregion
    }
}