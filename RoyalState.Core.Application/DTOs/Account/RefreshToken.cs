namespace RoyalState.Core.Application.DTOs.Account
{
    public class RefreshToken
    {
        public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Token { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime Revoked { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ReplaceByToken { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
        public bool IsActive => Revoked == null && !IsExpired;
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
    }
}
