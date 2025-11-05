namespace DefaultNamespace;

public class Token
{
    private TokenType type_; //тип токена
    private Tuple<int, int> index_; //содержание токена, индекс начала и конца
    TokenType TokenType
    {
        get { return type; }
        set { type = value; }
    }

    Tuple<int, int> Index
    {
        get { return index_; }
        set { index_ = value; }
    }
}