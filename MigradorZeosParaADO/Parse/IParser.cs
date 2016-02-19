namespace MigradorZeosParaADO.Parse
{
    public interface IParser<TReturn>
    {
        TReturn GetParsed(string toParse);
        string ToString(TReturn toConvert);
    }
}
