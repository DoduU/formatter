using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class JSSyntaxProvider
    {
        static List<string> tags = new List<string>();
        static List<char> specials = new List<char>();
        #region ctor
        static JSSyntaxProvider()
        {
            string[] strs = {
                "ADD"
                ,"EXTERNAL"	
                ,"PROCEDURE"
                ,"ALL"
                ,"FETCH"
                ,"PUBLIC"
                ,"ALTER"
                ,"FILE"
                ,"RAISERROR"
                ,"AND"
                ,"FILLFACTOR"
                ,"READ"
                ,"ANY"
                ,"FOR"
                ,"READTEXT"
                ,"AS"
                ,"FOREIGN"
                ,"RECONFIGURE"
                ,"ASC"
                ,"FREETEXT"
                ,"REFERENCES"
                ,"AUTHORIZATION"
                ,"FREETEXTTABLE"
                ,"REPLICATION"
                ,"BACKUP"
                ,"FROM"
                ,"RESTORE"
                ,"BEGIN"
                ,"FULL"
                ,"RESTRICT"
                ,"BETWEEN"
                ,"FUNCTION"
                ,"RETURN"
                ,"BREAK"
                ,"GOTO"
                ,"REVERT"
                ,"BROWSE"
                ,"GRANT"
                ,"REVOKE"
                ,"BULK"
                ,"GROUP"
                ,"RIGHT"
                ,"BY"
                ,"HAVING"
                ,"ROLLBACK"
                ,"CASCADE"
                ,"HOLDLOCK"
                ,"ROWCOUNT"
                ,"CASE"
                ,"IDENTITY"
                ,"ROWGUIDCOL"
                ,"CHECK"
                ,"IDENTITY_INSERT"
                ,"RULE"
                ,"CHECKPOINT"
                ,"IDENTITYCOL"
                ,"SAVE"
                ,"CLOSE"
                ,"IF"
                ,"SCHEMA"
                ,"CLUSTERED"
                ,"IN"
                ,"SECURITYAUDIT"
                ,"COALESCE"
                ,"INDEX"
                ,"SELECT"
                ,"COLLATE"
                ,"INNER"
                ,"SEMANTICKEYPHRASETABLE"
                ,"COLUMN"
                ,"INSERT"
                ,"SEMANTICSIMILARITYDETAILSTABLE"
                ,"COMMIT"
                ,"INTERSECT"
                ,"SEMANTICSIMILARITYTABLE"
                ,"COMPUTE"
                ,"INTO"
                ,"SESSION_USER"
                ,"CONSTRAINT"
                ,"IS"
                ,"SET"
                ,"CONTAINS"
                ,"JOIN"
                ,"SETUSER"
                ,"CONTAINSTABLE"
                ,"KEY"
                ,"SHUTDOWN"
                ,"CONTINUE"
                ,"KILL"
                ,"SOME"
                ,"CONVERT"
                ,"LEFT"
                ,"STATISTICS"
                ,"CREATE"
                ,"LIKE"
                ,"SYSTEM_USER"
                ,"CROSS"
                ,"LINENO"
                ,"TABLE"
                ,"CURRENT"
                ,"LOAD"
                ,"TABLESAMPLE"
                ,"CURRENT_DATE"
                ,"MERGE"
                ,"TEXTSIZE"
                ,"CURRENT_TIME"
                ,"NATIONAL"
                ,"THEN"
                ,"CURRENT_TIMESTAMP"
                ,"NOCHECK"
                ,"TO"
                ,"CURRENT_USER"
                ,"NONCLUSTERED"
                ,"TOP"
                ,"CURSOR"
                ,"NOT"
                ,"TRAN"
                ,"DATABASE"
                ,"NULL"
                ,"TRANSACTION"
                ,"DBCC"
                ,"NULLIF"
                ,"TRIGGER"
                ,"DEALLOCATE"
                ,"OF"
                ,"TRUNCATE"
                ,"DECLARE"
                ,"OFF"
                ,"TRY_CONVERT"
                ,"DEFAULT"
                ,"OFFSETS"
                ,"TSEQUAL"
                ,"DELETE"
                ,"ON"
                ,"UNION"
                ,"DENY"
                ,"OPEN"
                ,"UNIQUE"
                ,"DESC"
                ,"OPENDATASOURCE"
                ,"UNPIVOT"
                ,"DISK"
                ,"OPENQUERY"
                ,"UPDATE"
                ,"DISTINCT"
                ,"OPENROWSET"
                ,"UPDATETEXT"
                ,"DISTRIBUTED"
                ,"OPENXML"
                ,"USE"
                ,"DOUBLE"
                ,"OPTION"
                ,"USER"
                ,"DROP"
                ,"OR"
                ,"VALUES"
                ,"DUMP"
                ,"ORDER"
                ,"VARYING"
                ,"ELSE"
                ,"OUTER"
                ,"VIEW"
                ,"END"
                ,"OVER"
                ,"WAITFOR"
                ,"ERRLVL"
                ,"PERCENT"
                ,"WHEN"
                ,"ESCAPE"
                ,"PIVOT"
                ,"WHERE"
                ,"EXCEPT"
                ,"PLAN"
                ,"WHILE"
                ,"EXEC"
                ,"PRECISION"
                ,"WITH"
                ,"EXECUTE"
                ,"PRIMARY"
                ,"WITHIN GROUP"
                ,"EXISTS"
                ,"PRINT"
                ,"WRITETEXT"
                ,"EXIT"
                ,"PROC"
            };
            tags = new List<string>(strs);

            char[] chrs = {
                '.',
                ')',
                '(',
                '[',
                ']',
                '>',
                '<',
                ':',
                ';',
                '\n',
                '\t'
            };
            specials = new List<char>(chrs);
        }
        #endregion
        public static List<char> GetSpecials
        {
            get { return specials; }
        }
        public static List<string> GetTags
        {
            get { return tags; }
        }
        public static bool IsKnownTag(string tag)
        {
            return tags.Exists(delegate(string s) { return s.ToLower().Equals(tag.ToLower()); });
        }
        public static List<string> GetJSProvider(string tag)
        {
            return tags.FindAll(delegate(string s) { return s.ToLower().StartsWith(tag.ToLower()); });
        }
    }
}


