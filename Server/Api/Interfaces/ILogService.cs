using Api.Models;

namespace Api.Interfaces
{
    public interface ILogService
    {
        Note AddNote(string value, string status);
        Note GetNote(string id);
    }
}
