using HoloToolkit.Unity;
using System.Collections;

namespace HoloFlows.Client
{
    public class OpenHabRestClient : Singleton<OpenHabRestClient>
    {
        private const string ALL_ITEMS_POLL = "rest/items?recursive=false&fields=name%2C%20state";

        void Start()
        {

        }

        public IEnumerator GetAllItems()
        {
            yield return null;
        }
    }

}
