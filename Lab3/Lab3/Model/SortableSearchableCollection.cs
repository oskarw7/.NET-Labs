using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    //klasa ma byc uniwersalna hehe
    public class SortableSearchableCollection<T> : ObservableCollection<T>
    {
        public void SortBy<K>(Func<T, K> keySelector) where K : IComparable
        {
            var sorted = this.OrderBy(keySelector).ToList();
            this.Clear();
            foreach (var item in sorted)
            {
                this.Add(item);

            }

        }
        public List<T> SearchBy<K>(Func<T, K> selector, K value)
        {
            return this.Where(item => selector(item)?.Equals(value) == true).ToList();
        }
    }
}
