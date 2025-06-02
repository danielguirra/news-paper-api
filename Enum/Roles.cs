public static class Roles
{
    public const string Boss = "boss";
    public const string Admin = "admin";
    public const string Author = "author";
    public const string User = "user";

    public static int GetLevel(string role) =>
        role.ToLower() switch
        {
            Boss => 4,
            Admin => 3,
            Author => 2,
            User => 1,
            _ => 0,
        };
}
