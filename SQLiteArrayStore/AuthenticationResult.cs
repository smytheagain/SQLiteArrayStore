namespace SQLiteArrayStore
{
  /// <summary>
  /// Represents the authentication process result.
  /// </summary>
  public enum AuthenticationResult
  {
    /// <summary>
    /// The authentication has succeeded.
    /// </summary>
    Authenticated,

    /// <summary>
    /// The authentication has failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The authentication was cancelled by the user.
    /// </summary>
    Cancelled,
  }
}
