using contentservice.Dto;

namespace contentservice.Factories
{
    public class WebsiteCopyFactory : IWebsiteCopyFactory
    {
        public WebsiteCopyFactory() 
        {
        
        }

        public CtoCopyDto CreateCtoCopyDto(string copy, string copyTone, string copyAction, string copyContext)
        {
            return new CtoCopyDto { CopyId = Guid.NewGuid().ToString(),Copy = copy,CopyTone = copyTone,CopyAction = copyAction,CopyContext = copyContext};
        }
    }

    
}
