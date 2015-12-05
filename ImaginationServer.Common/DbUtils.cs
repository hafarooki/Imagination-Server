using System;
using System.Linq;
using ImaginationServer.Common.Data;
using NHibernate;

namespace ImaginationServer.Common
{
    /// <summary>
    ///     Utility class, used for managing the Redis database.
    /// </summary>
    public class DbUtils : IDisposable
    {
        public ISession Session { get; }

        public DbUtils()
        {
            Session = SessionHelper.CreateSession();
        }

        #region Character Stuff

        /// <summary>
        ///     Does a character of this ID exist?
        /// </summary>
        /// <param name="id">The ID of the supposed character.</param>
        /// <param name="notRaw">Is this not raw?</param>
        /// <returns>
        ///     Whether or not there is a character of that ID, and if there is, whether or not that character still actually
        ///     exists.
        /// </returns>
        public bool CharacterExists(long id, bool notRaw)
        {
            var characterId = id;
            if (!notRaw) characterId -= 1152921504606846994;
            return Session.CreateCriteria<Character>().List<Character>().Any(x => x.Id == characterId);
        }

        /// <summary>
        ///     Does a character of this username exist?
        /// </summary>
        /// <param name="username">The username of the supposed character.</param>
        /// <returns>Whether or not this character actually exists.</returns>
        public bool CharacterExists(string username)
        {
            return Session.CreateCriteria<Character>().List<Character>().Any(x => x.Name == username);
        }

        /// <summary>
        ///     Adds a character to the database.
        /// </summary>
        /// <param name="character">The character to add.</param>
        public void AddCharacter(Character character)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Save(character);
                transaction.Commit();
            }
        }

        /// <summary>
        ///     Update the data for an existing character.
        /// </summary>
        /// <param name="character">The character to update.</param>
        public void UpdateCharacter(Character character)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Update(character);
                transaction.Commit();
            }
        }

        /// <summary>
        ///     Removes a character from the database, and its owner account.
        /// </summary>
        /// <param name="character">The character to delete.</param>
        public void DeleteCharacter(Character character)
        {
            if (character.Owner == null || !AccountExists(character.Owner))
                return; // No need to do anything else if there is no valid account assigned to the specified character.
            var account = GetAccount(character.Owner);
            account.Characters.Remove(character.Name);
            UpdateAccount(account);
            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(character);
                transaction.Commit();
            }
        }

        /// <summary>
        ///     Gets a character from their ID.
        /// </summary>
        /// <param name="id">The character ID.</param>
        /// <param name="notRaw">Is this not raw?</param>
        /// <returns>The character</returns>
        public Character GetCharacter(long id, bool notRaw = false)
        {
            var characterId = id;
            if (!notRaw) characterId -= 1152921504606846994;
            return Session.Get<Character>(characterId);
        }

        /// <summary>
        ///     Gets a character from their name.
        /// </summary>
        /// <param name="name">The character's name.</param>
        /// <returns>The character</returns>
        public Character GetCharacter(string name)
        {
            return Session.CreateCriteria<Character>().List<Character>().Single(x => x.Name == name);
        }

        #endregion

        #region Account Stuff
        
        /// <summary>
        ///     Does an account of this username exist?
        /// </summary>
        /// <param name="username">The username of the supposed account.</param>
        /// <returns>Whether an account of the specified username exists.</returns>
        public bool AccountExists(string username)
        {
            return Session.CreateCriteria<Account>().List<Account>().Any(x => x.Username == username);
        }

        /// <summary>
        ///     Creates an account with specified username and password.
        /// </summary>
        /// <param name="username">The username of the account.</param>
        /// <param name="password">The password of the account.</param>
        /// <returns>The account that was created.</returns>
        public Account CreateAccount(string username, string password)
        {
            using (var transaction = Session.BeginTransaction())
            {
                var account = Account.Create(username, password);
                Session.SaveOrUpdate(account);
                transaction.Commit();
                return account;
            }
        }

        /// <summary>
        ///     Updates the data of an existing account. (Maybe you want to change the password, or add/remove users, or ban/unban
        ///     the account?)
        /// </summary>
        /// <param name="account">The account to update.</param>
        public void UpdateAccount(Account account)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(account);
                transaction.Commit();
            }
        }

        /// <summary>
        ///     Deletes the specified account.
        /// </summary>
        /// <param name="account">The account to delete.</param>
        public void DeleteAccount(Account account)
        {
            // Delete its characters
            foreach (var character in account.Characters)
            {
                if (!CharacterExists(character)) continue; // Maybe this character was deleted by error, or otherwise?
                DeleteCharacter(GetCharacter(character));
            }

            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(account);
                transaction.Commit();
            }
        }

        /// <summary>
        ///     Retrieves the data of account with the specified username.
        /// </summary>
        /// <param name="username">The username of the account.</param>
        /// <returns></returns>
        public Account GetAccount(string username)
        {
            return Session.CreateCriteria<Account>().List<Account>().Single(x => x.Username == username);
        }

        #endregion

        public void Dispose()
        {
            Session.Close();
        }
    }
}