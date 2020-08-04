using System.Collections.Generic;
using Util;

namespace Sample.Web.Service
{

    public interface IService<TViewModel>
    {
        List<TViewModel> GetAllList();
        PagingList<TViewModel> GetAll(int pageNumber = 1);
        TViewModel Get(long id);
        bool Insert(TViewModel model);
        bool Edit(TViewModel model);
        void Delete(long id);

        PagingList<TViewModel> GetAllbySearch(int pageNumber = 1, int pageSize = 10,
            Dictionary<string, dynamic> filterParams = null);

        IEnumerable<TViewModel> GetAllListbySearch(Dictionary<string, dynamic> filterParams, bool allIncluded = false);
        long InsertAndGetId(TViewModel model);

    }
}
