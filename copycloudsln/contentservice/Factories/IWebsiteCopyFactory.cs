using contentservice.Dto;

namespace contentservice.Factories
{
    public interface IWebsiteCopyFactory
    {
        CtoCopyDto CreateCtoCopyDto(string copy, string copyTone, string copyAction, string copyContext);
    }
}
