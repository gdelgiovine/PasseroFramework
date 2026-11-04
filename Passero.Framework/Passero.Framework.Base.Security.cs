using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Passero.Framework.Base
{
    public static class WellKnownPrincipalIds
    {
        public const string Everyone = "passero:everyone";
        public const string Administrators = "passero:administrators";
    }

    public static class AuthenticationBuiltinIds
    {
        public const string AdministratorUser = "Administrator";
        public const string GuestUser = "Guest";
        public const string AdministratorRole = "role:administrator";
        public const string AuthenticatedRole = "role:authenticated";
        public const string AnonymousRole = "role:anonymous";
        public const string AdministratorsGroup = "group:administrators";
        public const string GuestsGroup = "group:guests";
        public const string UsersGroup = "group:users";
    }

    public enum SecurityPrincipalType
    {
        User = 0,
        Group = 1
    }

    [Serializable]
    public sealed class SecurityPrincipal
    {
        public SecurityPrincipal(string id, string name, SecurityPrincipalType type)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("A principal identifier is required.", nameof(id));
            }

            if (!Enum.IsDefined(typeof(SecurityPrincipalType), type))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            Id = id.Trim();
            Name = name ?? string.Empty;
            Type = type;
            Enabled = true;
        }

        public string Id { get; }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public SecurityPrincipalType Type { get; }

        public bool Enabled { get; set; }
    }

    public enum SecurityScope
    {
        Application = 0,    
        Model = 1,
        ViewModel = 2,
        Repository = 3,
        Method = 4,
        Property = 5,
        UIObject = 6,
        View = 7,
        Report = 8
    }

    [Flags]
    public enum AccessPermission
    {
        None = 0,
        Read = 1,
        Execute = 2,
        Write = 4,
        Insert = 8,
        Delete = 16,
        FullControl = Read | Execute | Write | Insert | Delete
    }

    public enum AccessControlEffect
    {
        Allow = 0,
        Deny = 1
    }

    [Serializable]
    public sealed class AccessControlEntry
    {
        public AccessControlEntry(
            string principalId,
            SecurityScope scope,
            string resourceId,
            AccessPermission permissions,
            AccessControlEffect effect)
        {
            if (string.IsNullOrWhiteSpace(principalId))
            {
                throw new ArgumentException("A principal identifier is required.", nameof(principalId));
            }

            if (!Enum.IsDefined(typeof(SecurityScope), scope))
            {
                throw new ArgumentOutOfRangeException(nameof(scope));
            }

            if (string.IsNullOrWhiteSpace(resourceId))
            {
                throw new ArgumentException("A resource identifier is required.", nameof(resourceId));
            }

            if (permissions == AccessPermission.None || (permissions & ~AccessPermission.FullControl) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(permissions));
            }

            if (!Enum.IsDefined(typeof(AccessControlEffect), effect))
            {
                throw new ArgumentOutOfRangeException(nameof(effect));
            }

            PrincipalId = principalId.Trim();
            Scope = scope;
            ResourceId = resourceId.Trim();
            Permissions = permissions;
            Effect = effect;
        }

        public string PrincipalId { get; }

        public SecurityScope Scope { get; }

        public string ResourceId { get; }

        public AccessPermission Permissions { get; }

        public AccessControlEffect Effect { get; }
    }

    [Serializable]
    public sealed class AccessControlList
    {
        private readonly List<AccessControlEntry> _entries = new List<AccessControlEntry>();

        public IReadOnlyList<AccessControlEntry> Entries => _entries.AsReadOnly();

        public int Count => _entries.Count;

        public void Add(AccessControlEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            _entries.Add(entry);
        }

        public bool Remove(AccessControlEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return _entries.Remove(entry);
        }

        public bool IsAllowed(
            IEnumerable<string> effectivePrincipalIds,
            SecurityScope scope,
            string resourceId,
            AccessPermission requestedPermissions)
        {
            if (effectivePrincipalIds == null)
            {
                throw new ArgumentNullException(nameof(effectivePrincipalIds));
            }

            if (!Enum.IsDefined(typeof(SecurityScope), scope))
            {
                throw new ArgumentOutOfRangeException(nameof(scope));
            }

            if (string.IsNullOrWhiteSpace(resourceId))
            {
                throw new ArgumentException("A resource identifier is required.", nameof(resourceId));
            }

            if (requestedPermissions == AccessPermission.None ||
                (requestedPermissions & ~AccessPermission.FullControl) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(requestedPermissions));
            }

            var principalIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                WellKnownPrincipalIds.Everyone
            };

            foreach (var principalId in effectivePrincipalIds)
            {
                if (!string.IsNullOrWhiteSpace(principalId))
                {
                    principalIds.Add(principalId.Trim());
                }
            }

            var allowedPermissions = AccessPermission.None;
            var normalizedResourceId = resourceId.Trim();

            foreach (var entry in _entries)
            {
                if (entry.Scope != scope ||
                    !string.Equals(entry.ResourceId, normalizedResourceId, StringComparison.Ordinal) ||
                    !principalIds.Contains(entry.PrincipalId))
                {
                    continue;
                }

                if (entry.Effect == AccessControlEffect.Deny &&
                    (entry.Permissions & requestedPermissions) != AccessPermission.None)
                {
                    return false;
                }

                if (entry.Effect == AccessControlEffect.Allow)
                {
                    allowedPermissions |= entry.Permissions;
                }
            }

            return (allowedPermissions & requestedPermissions) == requestedPermissions;
        }
    }

    public interface ISecurityPrincipalProvider
    {
        IEnumerable<string> GetEffectivePrincipalIds();
    }

    public sealed class JsonSecurityIdentityDocument
    {
        public int SchemaVersion { get; set; } = 1;

        public List<SecurityRoleDefinition> Roles { get; set; } = new List<SecurityRoleDefinition>();

        public List<SecurityGroupDefinition> Groups { get; set; } = new List<SecurityGroupDefinition>();

        public List<SecurityUserDefinition> Users { get; set; } = new List<SecurityUserDefinition>();
    }

    public sealed class SecurityRoleDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsBuiltIn { get; set; }
    }

    public sealed class SecurityGroupDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public bool IsBuiltIn { get; set; }
    }

    public sealed class SecurityUserDefinition
    {
        public string UserName { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public List<string> Groups { get; set; } = new List<string>();

        public bool Enabled { get; set; } = true;

        public bool Lockout { get; set; }

        public bool IsBuiltIn { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public int PasswordIterations { get; set; }
    }

    public sealed class AuthenticationUserInfo
    {
        public string UserName { get; set; }

        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();

        public IReadOnlyList<string> Groups { get; set; } = Array.Empty<string>();

        public bool Enabled { get; set; }

        public bool Lockout { get; set; }

        public bool IsBuiltIn { get; set; }
    }

    public sealed class AuthenticatedSecurityIdentity
    {
        public AuthenticatedSecurityIdentity(string userName, string authenticationType)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("An authenticated username is required.", nameof(userName));

            UserName = userName.Trim();
            AuthenticationType = string.IsNullOrWhiteSpace(authenticationType)
                ? throw new ArgumentException("An authentication type is required.", nameof(authenticationType))
                : authenticationType.Trim();
        }

        public string UserName { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated => true;
    }

    public interface ILocalAuthenticationProvider
    {
        AuthenticatedSecurityIdentity Authenticate(string userName, string password);
    }

    public interface IAuthenticationUserStore : ILocalAuthenticationProvider
    {
        IReadOnlyList<AuthenticationUserInfo> GetUsers();

        AuthenticationUserInfo GetUser(string userName);

        void SaveUser(AuthenticationUserInfo user);

        bool DeleteUser(string userName);

        IReadOnlyList<SecurityRoleDefinition> GetRoles();

        void SaveRole(SecurityRoleDefinition role);

        bool DeleteRole(string roleId);

        IReadOnlyList<SecurityGroupDefinition> GetGroups();

        void SaveGroup(SecurityGroupDefinition group);

        bool DeleteGroup(string groupId);

        void SetUserStatus(string userName, bool enabled, bool lockout);

        void SetUserRoles(string userName, IEnumerable<string> roleIds);

        void SetUserGroups(string userName, IEnumerable<string> groupIds);

        void SetUserPassword(string userName, string password);
    }

    public static class AuthenticatedSecuritySession
    {
        private const string UserNameKey = "PASSERO_AUTHENTICATED_USER";
        private const string AuthenticationTypeKey = "PASSERO_AUTHENTICATION_TYPE";
        public static Func<AuthenticatedSecurityIdentity> CurrentIdentityResolver { get; set; }

        public static AuthenticatedSecurityIdentity Current
        {
            get
            {
                var userName = Wisej.Web.Application.Session[UserNameKey] as string;
                var authenticationType = Wisej.Web.Application.Session[AuthenticationTypeKey] as string;
                if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(authenticationType))
                    return new AuthenticatedSecurityIdentity(userName, authenticationType);

                return CurrentIdentityResolver?.Invoke();
            }
        }

        public static void SignIn(AuthenticatedSecurityIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            Wisej.Web.Application.Session[UserNameKey] = identity.UserName;
            Wisej.Web.Application.Session[AuthenticationTypeKey] = identity.AuthenticationType;
        }

        public static void SignOut()
        {
            Wisej.Web.Application.Session.Remove(UserNameKey);
            Wisej.Web.Application.Session.Remove(AuthenticationTypeKey);
        }
    }

    public static class LocalPasswordHasher
    {
        public const int DefaultIterations = 210000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static void CreateHash(string password, out string salt, out string passwordHash, int iterations = DefaultIterations)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("A password is required.", nameof(password));
            if (iterations < 100000)
                throw new ArgumentOutOfRangeException(nameof(iterations), "At least 100000 PBKDF2 iterations are required.");

            var saltBytes = new byte[SaltSize];
            using (var randomNumberGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider())
                randomNumberGenerator.GetBytes(saltBytes);

            salt = Convert.ToBase64String(saltBytes);
            passwordHash = Convert.ToBase64String(DeriveHash(password, saltBytes, iterations));
        }

        public static bool Verify(string password, string salt, string passwordHash, int iterations)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(salt) ||
                string.IsNullOrWhiteSpace(passwordHash) || iterations < 100000)
                return false;

            try
            {
                var expectedHash = Convert.FromBase64String(passwordHash);
                var saltBytes = Convert.FromBase64String(salt);
                var actualHash = DeriveHash(password, saltBytes, iterations);
                if (expectedHash.Length != actualHash.Length)
                    return false;

                int difference = 0;
                for (int i = 0; i < expectedHash.Length; i++)
                    difference |= expectedHash[i] ^ actualHash[i];
                return difference == 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] DeriveHash(string password, byte[] salt, int iterations)
        {
            using (var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                System.Security.Cryptography.HashAlgorithmName.SHA256))
            {
                return deriveBytes.GetBytes(HashSize);
            }
        }
    }

    public sealed class JsonSecurityPrincipalProvider : ISecurityPrincipalProvider, IAuthenticationUserStore
    {
        private const int CurrentSchemaVersion = 1;
        private readonly object _syncRoot = new object();
        private readonly Func<string> _currentUserNameProvider;
        private JsonSecurityIdentityDocument _document;
        private static readonly object FailedAttemptsLock = new object();
        private static readonly Dictionary<string, KeyValuePair<int, DateTime>> _failedAttempts =
            new Dictionary<string, KeyValuePair<int, DateTime>>(StringComparer.OrdinalIgnoreCase);
        private const string DummySalt = "AAAAAAAAAAAAAAAAAAAAAA==";
        private const string DummyHash = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

        public int MaxFailedAttempts { get; set; } = 5;

        public TimeSpan FailedAttemptsWindow { get; set; } = TimeSpan.FromMinutes(15);

        public bool AllowAdminAlias { get; set; }

        public JsonSecurityPrincipalProvider(string filePath, Func<string> currentUserNameProvider)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A JSON identity file path is required.", nameof(filePath));

            FilePath = Path.GetFullPath(filePath);
            _currentUserNameProvider = currentUserNameProvider ?? throw new ArgumentNullException(nameof(currentUserNameProvider));
            Reload();
        }

        public IReadOnlyList<AuthenticationUserInfo> GetUsers()
        {
            lock (_syncRoot)
            {
                return _document.Users.Select(ToUserInfo).ToArray();
            }
        }

        public AuthenticationUserInfo GetUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
                return user == null ? null : ToUserInfo(user);
            }
        }

        public void SaveUser(AuthenticationUserInfo user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new ArgumentException("A username is required.", nameof(user));

            lock (_syncRoot)
            {
                var existing = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, user.UserName.Trim(), StringComparison.OrdinalIgnoreCase));
                var roleIds = (user.Roles ?? Array.Empty<string>())
                    .Where(roleId => !string.IsNullOrWhiteSpace(roleId))
                    .Select(roleId => roleId.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                var groupIds = (user.Groups ?? Array.Empty<string>())
                    .Where(groupId => !string.IsNullOrWhiteSpace(groupId))
                    .Select(groupId => groupId.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                var copy = existing == null
                    ? new SecurityUserDefinition { UserName = user.UserName.Trim(), Roles = roleIds, Groups = groupIds }
                    : CloneUser(existing);
                if (copy.IsBuiltIn && !string.Equals(copy.UserName, user.UserName.Trim(), StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Builtin usernames cannot be changed.");
                if (copy.IsBuiltIn && existing != null &&
                    (!roleIds.SequenceEqual(existing.Roles, StringComparer.OrdinalIgnoreCase) ||
                     !groupIds.SequenceEqual(existing.Groups, StringComparer.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("Builtin user roles and groups are managed by the system.");
                copy.UserName = user.UserName.Trim();
                copy.Roles = roleIds;
                copy.Groups = groupIds;
                if (existing != null && !existing.IsBuiltIn)
                {
                    copy.Enabled = user.Enabled;
                    copy.Lockout = user.Lockout;
                }
                if (existing == null)
                {
                    AddDistinct(copy.Groups, AuthenticationBuiltinIds.UsersGroup);
                    _document.Users.Add(copy);
                }
                else
                    _document.Users[_document.Users.IndexOf(existing)] = copy;

                Validate(_document);
                SaveDocument(FilePath, _document);
            }
        }

        public IReadOnlyList<SecurityGroupDefinition> GetGroups()
        {
            lock (_syncRoot)
            {
                return _document.Groups.Select(group => new SecurityGroupDefinition
                {
                    Id = group.Id,
                    Name = group.Name,
                    Roles = (group.Roles ?? new List<string>()).ToList(),
                    IsBuiltIn = group.IsBuiltIn
                }).ToArray();
            }
        }

        public void SaveGroup(SecurityGroupDefinition group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            if (string.IsNullOrWhiteSpace(group.Id))
                throw new ArgumentException("A group ID is required.", nameof(group));

            lock (_syncRoot)
            {
                var existing = _document.Groups.FirstOrDefault(item =>
                    string.Equals(item.Id, group.Id.Trim(), StringComparison.OrdinalIgnoreCase));
                var roles = (group.Roles ?? new List<string>())
                    .Where(role => !string.IsNullOrWhiteSpace(role))
                    .Select(role => role.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                if (existing == null)
                {
                    var newGroup = new SecurityGroupDefinition { Id = group.Id.Trim(), Name = group.Name?.Trim(), Roles = roles };
                    _document.Groups.Add(newGroup);
                }
                else
                {
                    if (existing.IsBuiltIn)
                        throw new InvalidOperationException("Builtin groups cannot be modified.");
                    existing.Name = group.Name?.Trim();
                    existing.Roles = roles;
                }
                Validate(_document);
                SaveDocument(FilePath, _document);
            }
        }

        public bool DeleteGroup(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId))
                return false;
            lock (_syncRoot)
            {
                var group = _document.Groups.FirstOrDefault(item =>
                    string.Equals(item.Id, groupId.Trim(), StringComparison.OrdinalIgnoreCase));
                if (group == null)
                    return false;
                if (group.IsBuiltIn)
                    throw new InvalidOperationException("Builtin groups cannot be deleted.");
                if (_document.Users.Any(user => user.Groups.Any(id =>
                    string.Equals(id, group.Id, StringComparison.OrdinalIgnoreCase))))
                    throw new InvalidOperationException("Cannot delete a group assigned to users.");
                _document.Groups.Remove(group);
                SaveDocument(FilePath, _document);
                return true;
            }
        }

        public void SetUserStatus(string userName, bool enabled, bool lockout)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("A username is required.", nameof(userName));
            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (user == null)
                    throw new KeyNotFoundException($"User '{userName}' is not present in the identity document.");
                if (user.IsBuiltIn && string.Equals(user.UserName, AuthenticationBuiltinIds.GuestUser, StringComparison.OrdinalIgnoreCase))
                    enabled = false;
                if ((!enabled || lockout) && IsAdministrator(user))
                {
                    var anotherEnabledAdministrator = _document.Users.Any(candidate =>
                        !ReferenceEquals(candidate, user) && candidate.Enabled && !candidate.Lockout && IsAdministrator(candidate));
                    if (!anotherEnabledAdministrator)
                        throw new InvalidOperationException("At least one enabled, unlocked administrator must remain.");
                }
                user.Enabled = enabled;
                user.Lockout = lockout;
                SaveDocument(FilePath, _document);
            }
        }

        private bool IsAdministrator(SecurityUserDefinition user)
        {
            if (user.Roles.Any(role => string.Equals(role, AuthenticationBuiltinIds.AdministratorRole, StringComparison.OrdinalIgnoreCase)))
                return true;
            return user.Groups.Any(groupId =>
            {
                var group = _document.Groups.FirstOrDefault(item =>
                    string.Equals(item.Id, groupId, StringComparison.OrdinalIgnoreCase));
                return group != null && group.Roles.Any(role =>
                    string.Equals(role, AuthenticationBuiltinIds.AdministratorRole, StringComparison.OrdinalIgnoreCase));
            });
        }

        private static AuthenticationUserInfo ToUserInfo(SecurityUserDefinition user)
        {
            return new AuthenticationUserInfo
            {
                UserName = user.UserName,
                Roles = (user.Roles ?? new List<string>()).ToArray(),
                Groups = (user.Groups ?? new List<string>()).ToArray(),
                Enabled = user.Enabled,
                Lockout = user.Lockout,
                IsBuiltIn = user.IsBuiltIn
            };
        }

        public bool DeleteUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (user == null)
                    return false;
                if (user.IsBuiltIn)
                    throw new InvalidOperationException("Builtin users cannot be deleted.");

                _document.Users.Remove(user);
                SaveDocument(FilePath, _document);
                return true;
            }
        }

        public IReadOnlyList<SecurityRoleDefinition> GetRoles()
        {
            lock (_syncRoot)
            {
                return _document.Roles.Select(role => new SecurityRoleDefinition { Id = role.Id, Name = role.Name, IsBuiltIn = role.IsBuiltIn }).ToArray();
            }
        }

        public void SaveRole(SecurityRoleDefinition role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrWhiteSpace(role.Id))
                throw new ArgumentException("A role ID is required.", nameof(role));

            lock (_syncRoot)
            {
                var existing = _document.Roles.FirstOrDefault(item =>
                    string.Equals(item.Id, role.Id.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    var newRole = new SecurityRoleDefinition { Id = role.Id.Trim(), Name = role.Name?.Trim(), IsBuiltIn = role.IsBuiltIn };
                    _document.Roles.Add(newRole);
                }
                else
                {
                    if (existing.IsBuiltIn)
                        throw new InvalidOperationException("Builtin roles cannot be modified.");
                    existing.Name = role.Name?.Trim();
                }

                Validate(_document);
                SaveDocument(FilePath, _document);
            }
        }

        public bool DeleteRole(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                return false;

            lock (_syncRoot)
            {
                if (_document.Users.Any(user => user.Roles.Any(id =>
                    string.Equals(id, roleId.Trim(), StringComparison.OrdinalIgnoreCase))))
                    throw new InvalidOperationException("Cannot delete a role assigned to users.");

                if (_document.Groups.Any(group => group.Roles.Any(id =>
                    string.Equals(id, roleId.Trim(), StringComparison.OrdinalIgnoreCase))))
                    throw new InvalidOperationException("Cannot delete a role assigned to groups.");

                var role = _document.Roles.FirstOrDefault(item =>
                    string.Equals(item.Id, roleId.Trim(), StringComparison.OrdinalIgnoreCase));
                if (role == null)
                    return false;
                if (role.IsBuiltIn)
                    throw new InvalidOperationException("Builtin roles cannot be deleted.");

                _document.Roles.Remove(role);
                SaveDocument(FilePath, _document);
                return true;
            }
        }

        private static SecurityUserDefinition CloneUser(SecurityUserDefinition user)
        {
            return new SecurityUserDefinition
            {
                UserName = user.UserName,
                Roles = user.Roles?.ToList() ?? new List<string>(),
                Groups = user.Groups?.ToList() ?? new List<string>(),
                Enabled = user.Enabled,
                Lockout = user.Lockout,
                IsBuiltIn = user.IsBuiltIn,
                PasswordSalt = user.PasswordSalt,
                PasswordHash = user.PasswordHash,
                PasswordIterations = user.PasswordIterations
            };
        }

        public AuthenticatedSecurityIdentity Authenticate(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                return null;

            lock (FailedAttemptsLock)
            lock (_syncRoot)
            {
                var key = userName.Trim();
                if (_failedAttempts.Count > 1000)
                {
                    foreach (var expired in _failedAttempts.Where(item => DateTime.UtcNow - item.Value.Value > FailedAttemptsWindow).Select(item => item.Key).ToList())
                        _failedAttempts.Remove(expired);
                }

                if (_failedAttempts.TryGetValue(key, out var state))
                {
                    if (DateTime.UtcNow - state.Value > FailedAttemptsWindow)
                        _failedAttempts.Remove(key);
                    else if (MaxFailedAttempts > 0 && state.Key >= MaxFailedAttempts)
                        return null;
                }

                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, key, StringComparison.OrdinalIgnoreCase) ||
                    (AllowAdminAlias && string.Equals(key, "admin", StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(item.UserName, AuthenticationBuiltinIds.AdministratorUser, StringComparison.OrdinalIgnoreCase)));

                bool passwordValid = false;
                if (user != null)
                    passwordValid = LocalPasswordHasher.Verify(password, user.PasswordSalt, user.PasswordHash, user.PasswordIterations);
                else
                    LocalPasswordHasher.Verify(password, DummySalt, DummyHash, LocalPasswordHasher.DefaultIterations);

                if (user == null || !user.Enabled || user.Lockout || !passwordValid)
                {
                    var count = _failedAttempts.TryGetValue(key, out var previous) ? previous.Key : 0;
                    _failedAttempts[key] = new KeyValuePair<int, DateTime>(count + 1, DateTime.UtcNow);
                    return null;
                }

                _failedAttempts.Remove(key);
                return new AuthenticatedSecurityIdentity(user.UserName, "Local");
            }
        }

        public void SetUserPassword(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("A username is required.", nameof(userName));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("A password is required.", nameof(password));

            LocalPasswordHasher.CreateHash(password, out var salt, out var passwordHash, LocalPasswordHasher.DefaultIterations);
            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (user == null)
                    throw new KeyNotFoundException($"User '{userName}' is not present in the identity document.");

                user.PasswordSalt = salt;
                user.PasswordHash = passwordHash;
                user.PasswordIterations = LocalPasswordHasher.DefaultIterations;
                SaveDocument(FilePath, _document);
            }
        }

        public string FilePath { get; }

        public void Reload()
        {
            var document = JsonConvert.DeserializeObject<JsonSecurityIdentityDocument>(File.ReadAllText(FilePath));
            var originalJson = document == null ? null : JsonConvert.SerializeObject(document, Formatting.None);
            if (document != null && document.Groups == null)
                document.Groups = new List<SecurityGroupDefinition>();
            if (document != null && document.Users != null)
            {
                foreach (var user in document.Users)
                {
                    user.Groups = user.Groups ?? new List<string>();
                }
            }
            EnsureBuiltins(document);
            Validate(document);
            var normalizedJson = JsonConvert.SerializeObject(document, Formatting.None);
            lock (_syncRoot)
            {
                _document = document;
            }
            if (!string.Equals(originalJson, normalizedJson, StringComparison.Ordinal))
                SaveDocument(FilePath, document);
        }

        private static void EnsureBuiltins(JsonSecurityIdentityDocument document)
        {
            if (document == null)
                return;

            document.Roles = document.Roles ?? new List<SecurityRoleDefinition>();
            document.Groups = document.Groups ?? new List<SecurityGroupDefinition>();
            document.Users = document.Users ?? new List<SecurityUserDefinition>();

            AddBuiltinRole(document, AuthenticationBuiltinIds.AdministratorRole, "administrator");
            AddBuiltinRole(document, AuthenticationBuiltinIds.AuthenticatedRole, "authenticated");
            AddBuiltinRole(document, AuthenticationBuiltinIds.AnonymousRole, "anonymous");

            AddBuiltinGroup(document, AuthenticationBuiltinIds.AdministratorsGroup, "Administrators", AuthenticationBuiltinIds.AdministratorRole);
            AddBuiltinGroup(document, AuthenticationBuiltinIds.GuestsGroup, "Guests");
            AddBuiltinGroup(document, AuthenticationBuiltinIds.UsersGroup, "Users", AuthenticationBuiltinIds.AuthenticatedRole);

            var administrator = document.Users.FirstOrDefault(user =>
                string.Equals(user.UserName, AuthenticationBuiltinIds.AdministratorUser, StringComparison.OrdinalIgnoreCase));
            if (administrator == null)
            {
                administrator = new SecurityUserDefinition
                {
                    UserName = AuthenticationBuiltinIds.AdministratorUser,
                    Enabled = false,
                    IsBuiltIn = true
                };
                document.Users.Add(administrator);
            }
            administrator.IsBuiltIn = true;
            administrator.Groups = administrator.Groups ?? new List<string>();
            AddDistinct(administrator.Roles, AuthenticationBuiltinIds.AdministratorRole);
            AddDistinct(administrator.Groups, AuthenticationBuiltinIds.AdministratorsGroup);

            var guest = document.Users.FirstOrDefault(user =>
                string.Equals(user.UserName, AuthenticationBuiltinIds.GuestUser, StringComparison.OrdinalIgnoreCase));
            if (guest == null)
            {
                guest = new SecurityUserDefinition
                {
                    UserName = AuthenticationBuiltinIds.GuestUser,
                    Enabled = false,
                    IsBuiltIn = true
                };
                document.Users.Add(guest);
            }
            guest.IsBuiltIn = true;
            guest.Enabled = false;
            guest.Groups = guest.Groups ?? new List<string>();
            AddDistinct(guest.Groups, AuthenticationBuiltinIds.GuestsGroup);

            foreach (var user in document.Users.Where(user => !user.IsBuiltIn))
            {
                user.Groups = user.Groups ?? new List<string>();
                AddDistinct(user.Groups, AuthenticationBuiltinIds.UsersGroup);
            }
        }

        private static void AddBuiltinRole(JsonSecurityIdentityDocument document, string id, string name)
        {
            var role = document.Roles.FirstOrDefault(item => string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
            if (role == null)
            {
                role = new SecurityRoleDefinition { Id = id, Name = name };
                document.Roles.Add(role);
            }
            role.IsBuiltIn = true;

            if (string.Equals(id, AuthenticationBuiltinIds.AdministratorRole, StringComparison.OrdinalIgnoreCase))
            {
                var legacy = document.Roles.FirstOrDefault(item => string.Equals(item.Id, "role:admin", StringComparison.OrdinalIgnoreCase));
                if (legacy != null)
                {
                    foreach (var user in document.Users.Where(user => user.Roles.Any(value => string.Equals(value, legacy.Id, StringComparison.OrdinalIgnoreCase))))
                    {
                        ReplaceId(user.Roles, legacy.Id, id);
                    }
                    document.Roles.Remove(legacy);
                }
            }
        }

        private static void AddBuiltinGroup(JsonSecurityIdentityDocument document, string id, string name, params string[] roles)
        {
            var group = document.Groups.FirstOrDefault(item => string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
            if (group == null)
            {
                group = new SecurityGroupDefinition { Id = id, Name = name };
                document.Groups.Add(group);
            }
            group.IsBuiltIn = true;
            group.Roles = group.Roles ?? new List<string>();
            foreach (var role in roles)
                AddDistinct(group.Roles, role);
        }

        private static void AddDistinct(List<string> values, string value)
        {
            if (!values.Any(item => string.Equals(item, value, StringComparison.OrdinalIgnoreCase)))
                values.Add(value);
        }

        private static void ReplaceId(List<string> values, string oldId, string newId)
        {
            for (int index = 0; index < values.Count; index++)
            {
                if (string.Equals(values[index], oldId, StringComparison.OrdinalIgnoreCase))
                    values[index] = newId;
            }
        }

        public IEnumerable<string> GetEffectivePrincipalIds()
        {
            var authenticatedIdentity = AuthenticatedSecuritySession.Current;
            var userName = authenticatedIdentity?.UserName;
            if (string.IsNullOrWhiteSpace(userName))
                return new[] { WellKnownPrincipalIds.Everyone, AuthenticationBuiltinIds.AnonymousRole };

            userName = userName.Trim();
            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName, StringComparison.OrdinalIgnoreCase));
                if (user == null || !user.Enabled || user.Lockout)
                    return new[] { WellKnownPrincipalIds.Everyone, AuthenticationBuiltinIds.AnonymousRole };

                var principalIds = new List<string>
                {
                    WellKnownPrincipalIds.Everyone,
                    $"user:{user.UserName}",
                    AuthenticationBuiltinIds.AuthenticatedRole
                };
                principalIds.AddRange(user.Roles);
                foreach (var groupId in user.Groups)
                {
                    principalIds.Add(groupId);
                    var group = _document.Groups.FirstOrDefault(item =>
                        string.Equals(item.Id, groupId, StringComparison.OrdinalIgnoreCase));
                    if (group != null)
                        principalIds.AddRange(group.Roles ?? new List<string>());
                }
                return principalIds.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            }
        }

        public void SetUserRoles(string userName, IEnumerable<string> roleIds)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("A username is required.", nameof(userName));
            if (roleIds == null)
                throw new ArgumentNullException(nameof(roleIds));

            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (user == null)
                    throw new KeyNotFoundException($"User '{userName}' is not present in the identity document.");
                user.Roles = roleIds.Select(roleId => roleId?.Trim())
                    .Where(roleId => !string.IsNullOrWhiteSpace(roleId))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                Validate(_document);
                SaveDocument(FilePath, _document);
            }
        }

        public void SetUserGroups(string userName, IEnumerable<string> groupIds)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("A username is required.", nameof(userName));
            if (groupIds == null)
                throw new ArgumentNullException(nameof(groupIds));
            lock (_syncRoot)
            {
                var user = _document.Users.FirstOrDefault(item =>
                    string.Equals(item.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (user == null)
                    throw new KeyNotFoundException($"User '{userName}' is not present in the identity document.");
                if (user.IsBuiltIn)
                    throw new InvalidOperationException("Builtin group memberships are managed by the system.");
                user.Groups = groupIds.Select(groupId => groupId?.Trim())
                    .Where(groupId => !string.IsNullOrWhiteSpace(groupId))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                Validate(_document);
                SaveDocument(FilePath, _document);
            }
        }

        public static void Save(string filePath, JsonSecurityIdentityDocument document)
        {
            Validate(document);
            SaveDocument(filePath, document);
        }

        private static void SaveDocument(string filePath, JsonSecurityIdentityDocument document)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A JSON identity file path is required.", nameof(filePath));

            string fullPath = Path.GetFullPath(filePath);
            string directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            string temporaryPath = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                string json = JsonConvert.SerializeObject(document, Formatting.Indented);
                File.WriteAllText(temporaryPath, json, new UTF8Encoding(false));
                if (File.Exists(fullPath))
                    File.Replace(temporaryPath, fullPath, null);
                else
                    File.Move(temporaryPath, fullPath);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        private static void Validate(JsonSecurityIdentityDocument document)
        {
            if (document == null)
                throw new InvalidDataException("The security identity JSON document is empty or invalid.");
            if (document.SchemaVersion != CurrentSchemaVersion)
                throw new InvalidDataException($"Unsupported identity JSON schema version: {document.SchemaVersion}.");
            if (document.Roles == null || document.Users == null)
                throw new InvalidDataException("The identity JSON document must contain Roles and Users collections.");
            if (document.Groups == null)
                throw new InvalidDataException("The identity JSON document must contain a Groups collection.");

            var roleIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var role in document.Roles)
            {
                if (role == null || string.IsNullOrWhiteSpace(role.Id) || !role.Id.StartsWith("role:", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Every role must have a non-empty ID prefixed with 'role:'.");
                if (!roleIds.Add(role.Id.Trim()))
                    throw new InvalidDataException($"Duplicate role ID: '{role.Id}'.");
            }

            var groupIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var group in document.Groups)
            {
                if (group == null || string.IsNullOrWhiteSpace(group.Id) || !group.Id.StartsWith("group:", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Every group must have a non-empty ID prefixed with 'group:'.");
                if (!groupIds.Add(group.Id.Trim()))
                    throw new InvalidDataException($"Duplicate group ID: '{group.Id}'.");
                if (group.Roles == null)
                    throw new InvalidDataException($"The roles collection is required for group '{group.Id}'.");
                foreach (var roleId in group.Roles)
                {
                    if (string.IsNullOrWhiteSpace(roleId) || !roleIds.Contains(roleId.Trim()))
                        throw new InvalidDataException($"Group '{group.Id}' references an unknown role '{roleId}'.");
                }
            }

            var userNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var user in document.Users)
            {
                if (user == null || string.IsNullOrWhiteSpace(user.UserName))
                    throw new InvalidDataException("Every user must have a non-empty username.");
                if (!userNames.Add(user.UserName.Trim()))
                    throw new InvalidDataException($"Duplicate username: '{user.UserName}'.");
                if (user.Roles == null)
                    throw new InvalidDataException($"The roles collection is required for user '{user.UserName}'.");
                if (user.Groups == null)
                    throw new InvalidDataException($"The groups collection is required for user '{user.UserName}'.");

                foreach (var roleId in user.Roles)
                {
                    if (string.IsNullOrWhiteSpace(roleId) || !roleIds.Contains(roleId.Trim()))
                        throw new InvalidDataException($"User '{user.UserName}' references an unknown role '{roleId}'.");
                }
                foreach (var groupId in user.Groups)
                {
                    if (string.IsNullOrWhiteSpace(groupId) || !groupIds.Contains(groupId.Trim()))
                        throw new InvalidDataException($"User '{user.UserName}' references an unknown group '{groupId}'.");
                }
            }
        }
    }

    public interface IAccessControlListProvider
    {
        AccessControlList GetAccessControlList(SecurityScope scope, string resourceId);
    }

    public sealed class AccessControlEntryDefinition
    {
        public string PrincipalId { get; set; }

        public SecurityScope Scope { get; set; }

        public string ResourceId { get; set; }

        public AccessPermission Permissions { get; set; }

        public AccessControlEffect Effect { get; set; }

        public AccessControlEntry ToAccessControlEntry()
        {
            return new AccessControlEntry(PrincipalId, Scope, ResourceId, Permissions, Effect);
        }

        public static AccessControlEntryDefinition FromAccessControlEntry(AccessControlEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            return new AccessControlEntryDefinition
            {
                PrincipalId = entry.PrincipalId,
                Scope = entry.Scope,
                ResourceId = entry.ResourceId,
                Permissions = entry.Permissions,
                Effect = entry.Effect
            };
        }
    }

    public sealed class AccessControlListDocument
    {
        public const int CurrentSchemaVersion = 2;

        public int SchemaVersion { get; set; } = CurrentSchemaVersion;

        public List<AccessControlEntryDefinition> Entries { get; set; } = new List<AccessControlEntryDefinition>();

        public List<AccessControlDefaultDefinition> Defaults { get; set; } = new List<AccessControlDefaultDefinition>();

        public List<AccessControlExceptionDefinition> Exceptions { get; set; } = new List<AccessControlExceptionDefinition>();
    }

    public sealed class AccessControlRuleDefinition
    {
        public string PrincipalId { get; set; }

        public AccessPermission Permissions { get; set; }

        public AccessControlEffect Effect { get; set; }

        public IEnumerable<string> GetPrincipalIds()
        {
            return SplitPrincipalIds(PrincipalId)
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> SplitPrincipalIds(string value)
        {
            return (value ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(principalId => principalId.Trim())
                .Where(principalId => !string.IsNullOrWhiteSpace(principalId));
        }
    }

    public sealed class AccessControlDefaultDefinition
    {
        public SecurityScope Scope { get; set; }

        public string ResourcePattern { get; set; }

        public List<AccessControlRuleDefinition> Rules { get; set; } = new List<AccessControlRuleDefinition>();
    }

    public sealed class AccessControlExceptionDefinition
    {
        public SecurityScope Scope { get; set; }

        public string ResourceId { get; set; }

        public bool InheritDefaults { get; set; }

        public List<AccessControlRuleDefinition> Rules { get; set; } = new List<AccessControlRuleDefinition>();
    }

    public sealed class JsonAccessControlListProvider : IAccessControlListProvider
    {
        private readonly object _syncRoot = new object();
        private Dictionary<string, AccessControlList> _accessControlLists;
        private List<AccessControlDefaultDefinition> _defaults;

        public JsonAccessControlListProvider(string filePath, string currentUserPrincipalId = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A JSON file path is required.", nameof(filePath));

            FilePath = Path.GetFullPath(filePath);
            CurrentUserPrincipalId = string.IsNullOrWhiteSpace(currentUserPrincipalId)
                ? null
                : currentUserPrincipalId.Trim();
            Reload();
        }

        public string FilePath { get; }

        public string CurrentUserPrincipalId { get; }

        public AccessControlList GetAccessControlList(SecurityScope scope, string resourceId)
        {
            lock (_syncRoot)
            {
                _accessControlLists.TryGetValue(GetKey(scope, resourceId), out var acl);
                if (acl != null)
                    return acl;

                var matchingDefaults = _defaults
                    .Where(rule => rule.Scope == scope && MatchesResourcePattern(rule.ResourcePattern, resourceId))
                    .OrderByDescending(rule => GetPatternSpecificity(rule.ResourcePattern))
                    .ToList();
                if (matchingDefaults.Count == 0)
                    return null;

                acl = new AccessControlList();
                foreach (var definition in matchingDefaults)
                    AddRules(acl, definition.Rules, scope, resourceId, CurrentUserPrincipalId);
                return acl;
            }
        }

        public void Reload()
        {
            var json = File.ReadAllText(FilePath);
            var document = JsonConvert.DeserializeObject<AccessControlListDocument>(json, CreateJsonSettings());
            var accessControlLists = CreateAccessControlLists(document, CurrentUserPrincipalId, out var defaults);
            lock (_syncRoot)
            {
                _accessControlLists = accessControlLists;
                _defaults = defaults;
            }
        }

        public static AccessControlListDocument FromEntries(IEnumerable<AccessControlEntry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            return new AccessControlListDocument
            {
                SchemaVersion = AccessControlListDocument.CurrentSchemaVersion,
                Entries = entries.Select(AccessControlEntryDefinition.FromAccessControlEntry).ToList()
            };
        }

        public static AccessControlListDocument FromProvider(
            IAccessControlListProvider provider,
            IEnumerable<SecurityScope> scopes,
            IEnumerable<string> resourceIds)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (scopes == null)
                throw new ArgumentNullException(nameof(scopes));
            if (resourceIds == null)
                throw new ArgumentNullException(nameof(resourceIds));

            var entries = new List<AccessControlEntry>();
            foreach (var scope in scopes.Distinct())
            {
                foreach (var resourceId in resourceIds.Where(id => !string.IsNullOrWhiteSpace(id)).Distinct(StringComparer.Ordinal))
                {
                    var acl = provider.GetAccessControlList(scope, resourceId);
                    if (acl != null)
                        entries.AddRange(acl.Entries);
                }
            }

            return FromEntries(entries);
        }

        public static string Serialize(AccessControlListDocument document)
        {
            ValidateDocument(document);
            return JsonConvert.SerializeObject(document, Formatting.Indented, CreateJsonSettings());
        }

        public static string Serialize(IEnumerable<AccessControlEntry> entries)
        {
            return Serialize(FromEntries(entries));
        }

        public static void Save(string filePath, AccessControlListDocument document)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("A JSON file path is required.", nameof(filePath));

            var fullPath = Path.GetFullPath(filePath);
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            var temporaryPath = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                File.WriteAllText(temporaryPath, Serialize(document), new UTF8Encoding(false));
                if (File.Exists(fullPath))
                    File.Replace(temporaryPath, fullPath, null);
                else
                    File.Move(temporaryPath, fullPath);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        public static void Save(string filePath, IEnumerable<AccessControlEntry> entries)
        {
            Save(filePath, FromEntries(entries));
        }

        public static void Save(string filePath, IAccessControlListProvider provider,
            IEnumerable<SecurityScope> scopes, IEnumerable<string> resourceIds)
        {
            Save(filePath, FromProvider(provider, scopes, resourceIds));
        }

        private static Dictionary<string, AccessControlList> CreateAccessControlLists(
            AccessControlListDocument document,
            string currentUserPrincipalId,
            out List<AccessControlDefaultDefinition> defaults)
        {
            ValidateDocument(document);
            var result = new Dictionary<string, AccessControlList>(StringComparer.Ordinal);
            foreach (var definition in document.Entries ?? new List<AccessControlEntryDefinition>())
            {
                var principalId = string.Equals(definition.PrincipalId, "$currentUser", StringComparison.Ordinal)
                    ? currentUserPrincipalId
                    : definition.PrincipalId;
                if (string.IsNullOrWhiteSpace(principalId))
                    continue;

                var entry = new AccessControlEntry(
                    principalId,
                    definition.Scope,
                    definition.ResourceId,
                    definition.Permissions,
                    definition.Effect);
                var key = GetKey(entry.Scope, entry.ResourceId);
                if (!result.TryGetValue(key, out var acl))
                {
                    acl = new AccessControlList();
                    result.Add(key, acl);
                }
                acl.Add(entry);
            }

            foreach (var exception in document.Exceptions ?? new List<AccessControlExceptionDefinition>())
            {
                var acl = new AccessControlList();
                if (exception.InheritDefaults)
                {
                    foreach (var definition in (document.Defaults ?? new List<AccessControlDefaultDefinition>())
                        .Where(item => item.Scope == exception.Scope &&
                            MatchesResourcePattern(item.ResourcePattern, exception.ResourceId))
                        .OrderByDescending(item => GetPatternSpecificity(item.ResourcePattern)))
                    {
                        AddRules(acl, definition.Rules, exception.Scope, exception.ResourceId, currentUserPrincipalId);
                    }
                }
                AddRules(acl, exception.Rules, exception.Scope, exception.ResourceId, currentUserPrincipalId);
                result[GetKey(exception.Scope, exception.ResourceId)] = acl;
            }

            defaults = (document.Defaults ?? new List<AccessControlDefaultDefinition>())
                .Select(definition => new AccessControlDefaultDefinition
                {
                    Scope = definition.Scope,
                    ResourcePattern = definition.ResourcePattern,
                    Rules = definition.Rules
                        .SelectMany(rule => rule.GetPrincipalIds().Select(principalId =>
                            new AccessControlRuleDefinition
                            {
                                PrincipalId = string.Equals(principalId, "$currentUser", StringComparison.Ordinal)
                                    ? currentUserPrincipalId
                                    : principalId,
                                Permissions = rule.Permissions,
                                Effect = rule.Effect
                            }))
                        .Where(rule => !string.IsNullOrWhiteSpace(rule.PrincipalId))
                        .ToList()
                })
                .ToList();
            return result;
        }

        private static void AddRules(
            AccessControlList acl,
            IEnumerable<AccessControlRuleDefinition> rules,
            SecurityScope scope,
            string resourceId,
            string currentUserPrincipalId = null)
        {
            foreach (var rule in rules ?? Enumerable.Empty<AccessControlRuleDefinition>())
            {
                foreach (var sourcePrincipalId in rule.GetPrincipalIds())
                {
                    var principalId = string.Equals(sourcePrincipalId, "$currentUser", StringComparison.Ordinal)
                        ? currentUserPrincipalId
                        : sourcePrincipalId;
                    if (string.IsNullOrWhiteSpace(principalId))
                        continue;

                    acl.Add(new AccessControlEntry(
                        principalId,
                        scope,
                        resourceId,
                        rule.Permissions,
                        rule.Effect));
                }
            }
        }

        private static bool MatchesResourcePattern(string pattern, string resourceId)
        {
            if (string.IsNullOrWhiteSpace(pattern) || string.IsNullOrWhiteSpace(resourceId))
                return false;

            pattern = pattern.Trim();
            resourceId = resourceId.Trim();
            if (pattern == "*")
                return true;
            if (pattern.EndsWith(".*", StringComparison.Ordinal))
            {
                var prefix = pattern.Substring(0, pattern.Length - 1);
                return resourceId.StartsWith(prefix, StringComparison.Ordinal);
            }

            return string.Equals(pattern, resourceId, StringComparison.Ordinal);
        }

        private static int GetPatternSpecificity(string pattern)
        {
            return pattern == "*" ? 0 : pattern.EndsWith(".*", StringComparison.Ordinal) ? pattern.Length - 2 : int.MaxValue;
        }

        private static void ValidateDocument(AccessControlListDocument document)
        {
            if (document == null)
                throw new InvalidDataException("The ACL JSON document is empty or invalid.");
            if (document.SchemaVersion < 1 || document.SchemaVersion > AccessControlListDocument.CurrentSchemaVersion)
                throw new InvalidDataException($"Unsupported ACL JSON schema version: {document.SchemaVersion}.");

            foreach (var definition in document.Entries ?? new List<AccessControlEntryDefinition>())
            {
                if (definition == null)
                    throw new InvalidDataException("The ACL JSON contains a null entry.");
                if (!Enum.IsDefined(typeof(SecurityScope), definition.Scope))
                    throw new InvalidDataException($"Invalid ACL scope: {definition.Scope}.");
                if (!Enum.IsDefined(typeof(AccessControlEffect), definition.Effect))
                    throw new InvalidDataException($"Invalid ACL effect: {definition.Effect}.");
                definition.ToAccessControlEntry();
            }

            foreach (var definition in document.Defaults ?? new List<AccessControlDefaultDefinition>())
            {
                if (!Enum.IsDefined(typeof(SecurityScope), definition.Scope))
                    throw new InvalidDataException($"Invalid default ACL scope: {definition.Scope}.");
                if (string.IsNullOrWhiteSpace(definition.ResourcePattern))
                    throw new InvalidDataException("A default ACL resource pattern is required.");
                ValidateRules(definition.Rules);
            }

            foreach (var definition in document.Exceptions ?? new List<AccessControlExceptionDefinition>())
            {
                if (!Enum.IsDefined(typeof(SecurityScope), definition.Scope))
                    throw new InvalidDataException($"Invalid exception ACL scope: {definition.Scope}.");
                if (string.IsNullOrWhiteSpace(definition.ResourceId))
                    throw new InvalidDataException("An exception ACL resource identifier is required.");
                ValidateRules(definition.Rules);
            }
        }

        private static void ValidateRules(IEnumerable<AccessControlRuleDefinition> rules)
        {
            if (rules == null)
                throw new InvalidDataException("ACL rules collection is required.");

            foreach (var rule in rules)
            {
                if (rule == null)
                    throw new InvalidDataException("The ACL JSON contains a null rule.");
                var principalIds = rule.GetPrincipalIds().ToList();
                if (principalIds.Count == 0)
                    throw new InvalidDataException("Each ACL rule must specify PrincipalId or a non-empty PrincipalIds array.");
                foreach (var principalId in principalIds)
                {
                    new AccessControlEntry(
                        principalId,
                        SecurityScope.Application,
                        "validation",
                        rule.Permissions,
                        rule.Effect);
                }
            }
        }

        private static string GetKey(SecurityScope scope, string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                throw new ArgumentException("A resource identifier is required.", nameof(resourceId));
            return $"{scope}|{resourceId.Trim()}";
        }

        private static JsonSerializerSettings CreateJsonSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            return settings;
        }
    }

    public interface ISecurityService
    {
        bool IsEnabled { get; }

        bool IsAllowed(SecurityScope scope, string resourceId, AccessPermission requestedPermissions);
    }

    public sealed class DisabledSecurityService : ISecurityService
    {
        public static readonly DisabledSecurityService Instance = new DisabledSecurityService();

        private DisabledSecurityService()
        {
        }

        public bool IsEnabled => false;

        public bool IsAllowed(SecurityScope scope, string resourceId, AccessPermission requestedPermissions) => true;
    }

    internal sealed class UnavailableSecurityService : ISecurityService
    {
        public static readonly UnavailableSecurityService Instance = new UnavailableSecurityService();

        private UnavailableSecurityService()
        {
        }

        public bool IsEnabled => true;

        public bool IsAllowed(SecurityScope scope, string resourceId, AccessPermission requestedPermissions) => false;
    }

    public sealed class AccessControlSecurityService : ISecurityService
    {
        private readonly ISecurityPrincipalProvider _principalProvider;
        private readonly IAccessControlListProvider _accessControlListProvider;

        public AccessControlSecurityService(
            ISecurityPrincipalProvider principalProvider,
            IAccessControlListProvider accessControlListProvider)
        {
            _principalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
            _accessControlListProvider = accessControlListProvider ?? throw new ArgumentNullException(nameof(accessControlListProvider));
        }

        public bool IsEnabled => true;

        public bool IsAllowed(SecurityScope scope, string resourceId, AccessPermission requestedPermissions)
        {
            var accessControlList = _accessControlListProvider.GetAccessControlList(scope, resourceId);
            if (accessControlList == null)
            {
                return false;
            }

            var principalIds = _principalProvider.GetEffectivePrincipalIds();
            return principalIds != null && accessControlList.IsAllowed(principalIds, scope, resourceId, requestedPermissions);
        }
    }

    public interface ISecurityAwareRepository
    {
        ISecurityService SecurityService { get; set; }
    }

    public static class SecurityAuthorization
    {
        /// <summary>Se true, un servizio di sicurezza mancante (null) nega l'accesso invece di ammetterlo.</summary>
        public static bool FailClosedWhenServiceMissing { get; set; }

        public static void Demand(ISecurityService securityService, SecurityScope scope, string resourceId, AccessPermission requestedPermissions)
        {
            if (securityService == null && FailClosedWhenServiceMissing)
            {
                throw new UnauthorizedAccessException(
                    $"Access denied: {requestedPermissions} on {scope} '{resourceId}' (no security service configured).");
            }

            if (securityService == null || !securityService.IsEnabled)
            {
                return;
            }

            if (!securityService.IsAllowed(scope, resourceId, requestedPermissions))
            {
                throw new UnauthorizedAccessException(
                    $"Access denied: {requestedPermissions} on {scope} '{resourceId}'.");
            }
        }

        public static void DemandRepositoryOperation(
            ISecurityService securityService,
            Type modelType,
            Type viewModelType,
            AccessPermission requestedPermissions)
        {
            if (securityService == null || !securityService.IsEnabled)
            {
                return;
            }

            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var modelResourceId = GetResourceId(modelType);
            Demand(securityService, SecurityScope.Model, modelResourceId, requestedPermissions);
            Demand(securityService, SecurityScope.Repository, modelResourceId, requestedPermissions);

            if (viewModelType != null)
            {
                Demand(securityService, SecurityScope.ViewModel, GetResourceId(viewModelType), requestedPermissions);
            }
        }

        public static string GetResourceId(Type resourceType)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException(nameof(resourceType));
            }

            return resourceType.FullName ?? resourceType.Name;
        }
    }
}
