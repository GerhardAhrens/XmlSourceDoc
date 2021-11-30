#pragma warning disable CS1591

namespace XmlSourceDocDemo
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class InMemoryRepository<TDomain> : IRepository<TDomain> where TDomain : IDomainRoot
    {
        private readonly ConcurrentDictionary<Type, TDomain> memorySource = null;
        public InMemoryRepository()
        {
            this.memorySource = new ConcurrentDictionary<Type, TDomain>();
        }

        public int CountAll()
        {
            return this.memorySource.Count;
        }

        public int CountByType()
        {
            int result = 0;
            if (memorySource != null)
            {
                Type typ = typeof(TDomain);
                result = this.memorySource.Count(w => w.Key == typ);
            }

            return result;
        }

        public TDomain FindById(Guid id)
        {
            TDomain result = default(TDomain);

            if (this.memorySource != null)
            {
                if (this.memorySource.Any(a => a.Value.Id == id))
                {
                    result = (TDomain)this.memorySource.Where(w => w.Value.Id == id).FirstOrDefault().Value;
                }
            }

            return result;
        }

        public void Add(TDomain domainObj)
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                this.memorySource.TryAdd(typ, domainObj);
            }
        }

        public void Update(TDomain domainObj)
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                TDomain oldDomain = this.memorySource.Where(w => w.Value.Id == domainObj.Id).FirstOrDefault().Value;
                this.memorySource.TryUpdate(typ, domainObj, (TDomain)oldDomain);
            }
        }

        public void Delete(TDomain domainObj)
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                this.memorySource.TryRemove(typ, out domainObj);
            }
        }

        public void DeleteAllByType()
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                List<TDomain> byType = this.memorySource.Where(c => c.Key == typ).Select(s => s.Value).ToList();
                for (int i = 0; i < byType.Count; i++)
                {
                    TDomain oldDomain = this.memorySource.Where(w => w.Value.Id == byType[i].Id).FirstOrDefault().Value;
                    this.memorySource.TryRemove(typ, out oldDomain);
                }
            }
        }

        public void DeleteAll()
        {
            if (this.memorySource != null)
            {
                Type typ = typeof(TDomain);
                this.memorySource.Clear();
            }
        }
    }

    public interface IDomainRoot
    {
        Guid Id { get; }
    }

    public interface IRepository<TDomain> where TDomain : IDomainRoot
    {
        TDomain FindById(Guid id);

        void Add(TDomain domainObj);

        void Update(TDomain domainObj);

        void Delete(TDomain domainObj);
    }

    public sealed class MemoryContent<TDomain> : Tuple<Type, TDomain>
    {
        public MemoryContent(Type key, TDomain value) : base(key,value)
        {
        }

        public Type Key
        {
            get
            {
                return this.Item1;
            }
        }

        public TDomain Value
        {
            get
            {
                return this.Item2;
            }
        }
    }
}
