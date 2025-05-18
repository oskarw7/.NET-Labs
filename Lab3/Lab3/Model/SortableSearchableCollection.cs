using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    //klasa ma byc uniwersalna 
    public class SortableSearchableCollection<T> : ObservableCollection<T>
    {
        private List<T> originalItems = new List<T> { };
        public void SortBy<K>(Func<T, K> keySelector) where K : IComparable
        {
            var sorted = this.OrderBy(keySelector).ToList();
            this.Clear();
            foreach (var item in sorted)
            {
                this.Add(item);

            }

        }
        public void SearchBy(Func<T, bool> predicate)
        {
            this.Clear();
            foreach (var item in originalItems.Where(predicate).ToList())
            {
                this.Add(item);
            }
        }

        public void SaveOriginal()
        {
            this.originalItems.Clear();
            this.originalItems = this.ToList();
        }

        public void RevertOriginal()
        {
            this.Clear();
            foreach (var item in originalItems.ToList())
            {
                this.Add(item);
            }
        }

        public void AddToOriginal(T item)
        {
            this.originalItems.Add(item);
        }

        public void RemoveFromOriginal(T item)
        {
            this.originalItems.Remove(item);
        }
    }
}
