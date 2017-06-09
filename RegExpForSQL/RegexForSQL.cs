using System;
//using System.Data;
//using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
//using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

public partial class RegexForSQL
{
    //options
    //0     None	                Specifies that no options are set.
    //1	    IgnoreCase	            Specifies case-insensitive matching.
    //2	    Multiline	            Multiline mode. Changes the meaning of ^ and $ so they match at the beginning and end, respectively, of any line, and not just the beginning and end of the entire string.
    //4	    ExplicitCapture	        Specifies that the only valid captures are explicitly named or numbered groups of the form (?<name>...). This allows unnamed parentheses to act as non-capturing groups without the syntactic clumsiness of the expression (?:...).
    //8 	Compiled	            Specifies that the Regular Expression is compiled to an assembly. This yields faster execution, but increases the startup time. This value should not be assigned to the Options property when calling the CompileToAssembly method.
    //16	Singleline	            Specifies the single-line mode. Changes the meaning of the dot (.) so it matches every character (instead of every character except \n).
    //32	IgnorePatternWhitespace	Eliminates unescaped white space from the pattern, and enables comments marked with #. However, the IgnorePatternWhitespace value does not affect or eliminate white space in character classes.
    //64	RightToLeft	            Specifies that the search will be from right to left instead of from left to right.
    //256	ECMAScript	            Enables ECMAScript-compliant behavior for the expression. This value can be used only in conjunction with the IgnoreCase, Multiline, and Compiled values. The use of this value with any other value results in an exception.
    //512	CultureInvariant	    Specifies that cultural differences in language are ignored.
    [Microsoft.SqlServer.Server.SqlFunction(Name = "RegexIsMatch", IsDeterministic = true, IsPrecise = true)]
    public static SqlBoolean RegexIsMatch(SqlString input, SqlString pattern, SqlInt32 options)
    {
        if (input.IsNull)
        {
            return SqlBoolean.Null;
        }
        
        if (pattern.IsNull)
        {
            pattern = String.Empty;
        }

        if (options.IsNull)
        {
            options = 0;
        }

        try
        {
            bool match = Regex.IsMatch((string)input, (string)pattern, (RegexOptions)(int)options);
            
            return (SqlBoolean)match;
        }
        catch
        {
            throw;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction(Name = "RegexMatch", IsDeterministic = true, IsPrecise = true)]
    public static SqlString RegexMatch(SqlString input, SqlString pattern, SqlString groupName, SqlInt32 options)
    {
        if (input.IsNull)
        {
            return SqlString.Null;
        }

        if (pattern.IsNull)
        {
            pattern = String.Empty;
        }

        if (groupName.IsNull)
        {
            groupName = "1";
        }

        if (options.IsNull)
        {
            options = 0;
        }

        try
        {
            Match match = Regex.Match((string)input, (string)pattern, (RegexOptions)(int)options);
            if (match.Success)
            {
                return (SqlString)match.Groups[groupName.Value].Value;
            }
            
        }
        catch
        {
            throw;
        }

        return SqlString.Null;
    }

    [Microsoft.SqlServer.Server.SqlFunction(Name = "RegexReplace", IsDeterministic = true, IsPrecise = true)]
    public static SqlString RegexReplace(SqlString input, SqlString pattern, SqlString replacement, SqlInt32 options)
    {
        if (input.IsNull)
        {
            return SqlString.Null;
        }

        if (pattern.IsNull)
        {
            pattern = String.Empty;
        }

        if (replacement.IsNull)
        {
            replacement = String.Empty;
        }

        if (options.IsNull)
        {
            options = 0;
        }

        try
        {
            string match = Regex.Replace((string)input, (string)pattern, (string)replacement, (RegexOptions)(int)options);

            return (SqlString)match;            
        }
        catch
        {
            throw;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction(Name = "RegexMatches", IsDeterministic = true, IsPrecise = true, FillRowMethodName = "FillMatches", TableDefinition="idx int, value nvarchar(4000), length int")]
    public static IEnumerable RegexMatches(SqlString input, SqlString pattern, SqlInt32 options)
    {
        if (pattern.IsNull)
        {
            pattern = String.Empty;
        }

        if (options.IsNull)
        {
            options = 0;
        }

        try
        {
            MatchCollection matches = Regex.Matches((string)input, (string)pattern, (RegexOptions)(int)options);

            return matches;
        }
        catch
        {
            throw;
        }
    }

    public static void FillMatches(object obj, out SqlInt32 idx, out SqlString value, out SqlInt32 length)
    {
        Match match = (Match)obj;
        idx = (SqlInt32)match.Index;
        value = (SqlString)match.Value;
        length = (SqlInt32)match.Length;

    }
}
