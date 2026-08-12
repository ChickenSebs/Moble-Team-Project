using MySql.Data.MySqlClient;

namespace tap // 회원가입 폼과 동일한 네임스페이스
{
    public static class DBHelper
    {
        // 本人의 MySQL 정보로 수정하세요 (Server, Port, Database, Uid, Pwd)
        private static string connectionString =
            "Server=localhost;Port=3306;Database=teamproject;Uid=root;Pwd=1111;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}