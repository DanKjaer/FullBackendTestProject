using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure;

public class Repository
{
    private readonly NpgsqlDataSource _dataSource;

    public Repository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Book> GetAllBooks()
    {
        var sql = $@"select * from library.books;";
        using(var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Book>(sql);
        }
    }


    public Book CreateBook(Book book)
    {
        var sql = $@"INSERT INTO library.books (title, publisher, coverImgUrl)
                     VALUES (@title, @publisher, @coverImgUrl) RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Book>(sql, new { book.Title, book.Publisher, book.CoverImgUrl });
        }
    }
    
    public Book UpdateBook(Book book, int bookId)
    {
        var sql = $@"UPDATE library.books set
                        title = @title,
                        publisher = @publisher,
                        coverImgUrl = @coverImgUrl
                        WHERE bookId = @bookId RETURNING   
                        
                            *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Book>(sql, new { book.Title, book.Publisher, book.CoverImgUrl, book.BookId });
        }
    }
    
    public object DeleteBook(int bookId)
    {
        var sql = $@"DELETE FROM library.books WHERE bookId = @bookId";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(sql, new { bookId }) == 1;
        }
    }


}