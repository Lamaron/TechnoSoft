using Data.InMemory;
namespace _Data
{
    public static class RepositoryContainer
    {
        private static RequestRepository _requestRepository;

        public static RequestRepository RequestRepository
        {
            get
            {
                if (_requestRepository == null)
                {
                    _requestRepository = new RequestRepository();
                }
                return _requestRepository;
            }
        }
    }
}
