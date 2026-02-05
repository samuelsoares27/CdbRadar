namespace CdbRadar.Repository.Interfaces
{
    public interface ITipoAtivoRepository
    {
        Task<int?> ObterIdPorNomeAsync(string nome);
    }
}
