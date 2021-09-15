using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaricardodev.Paginator.Model.Capabilities;
using Xunit;

namespace Model.Test
{
    public class PaginatedListTests : IDisposable
    {
        private PaginatedList<dynamic> _paginatedList;

        private dynamic _listItem;

        public PaginatedListTests()
        {
            _listItem = GetTestItem();
            _paginatedList = new PaginatedList<dynamic> { _listItem };
        }
        
        
        private dynamic GetTestItem()
        {
            return new
            {
                Title = "Test Post Title",
                Content = "Test Post Content",
                EndDate = DateTime.UtcNow,
                ImageUrl = "url",
                IsLikeAllowed = true
            };
        }

        [Fact]
        public void GetEnumerator_ReturnsTheEnumerator()
        {
            var enumerator = _paginatedList.GetEnumerator();
            Assert.NotNull(enumerator);
        }


        [Fact]
        public void Clear_ClearTheList()
        {
            Assert.Single(_paginatedList);
            _paginatedList.Clear();
            Assert.Empty(_paginatedList);
        }

        [Fact]
        public void Contains_ReturnsTrueIfContainsTheItem()
        {
            var contains = _paginatedList.Contains(_listItem);
            Assert.True(contains);
        }

        [Fact]
        public void Remove_ReturnsTrueIfItemRemoved()
        {
            var removed = _paginatedList.Remove(_listItem);
            var contains = _paginatedList.Contains(_listItem);

            Assert.True(removed);
            Assert.False(contains);
        }

        [Fact]
        public void IsReadOnly_ReturnsFalse()
        {
            var isReadOnly = _paginatedList.IsReadOnly;
            Assert.False(isReadOnly);
        }

        [Fact]
        public void IndexOf_ReturnsTheIndex()
        {
            var index = _paginatedList.IndexOf(_listItem);
            Assert.Equal(0, index);
        }

        [Fact]
        public void Insert_InsertTheItemAtTheIndex()
        {
            var secondItem = GetTestItem();
            _paginatedList.Insert(1, secondItem);

            var index = _paginatedList.IndexOf(secondItem);
            Assert.Equal(1, index);
            Assert.Equal(2, _paginatedList.Count);
        }

        [Fact]
        public void RemoveAt_RemoveItemAtTheIndex()
        {
            _paginatedList.RemoveAt(0);
            var index = _paginatedList.IndexOf(_listItem);

            Assert.Equal(-1, index);
            Assert.Empty(_paginatedList);
        }

        [Fact]
        public void Index_SetAValueAtTheIndex()
        {
            var secondItem = GetTestItem();
            _paginatedList[0] = secondItem;

            var index = _paginatedList.IndexOf(secondItem);
            Assert.Equal(0, index);
        }

        public void Dispose()
        {
            _paginatedList.Clear();
        }
    }
}
