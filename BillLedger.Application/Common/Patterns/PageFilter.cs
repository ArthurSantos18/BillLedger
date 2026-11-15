namespace BillLedger.Application.Common.Patterns;

public class PageFilter
{
    [Description("O número da página dos resultados. O valor padrão é 1.")]
    public int Page { get; set; } = 1;

    [Description("A quantidade máxima de registros retornados por página. O valor padrão é 60.")]
    public int PageSize { get; set; } = 60;

    public int Skip => (Page - 1) * PageSize;
}