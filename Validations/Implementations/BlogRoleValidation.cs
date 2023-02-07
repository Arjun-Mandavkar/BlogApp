using BlogApp.Models;
using BlogApp.Repositories;

namespace BlogApp.Validations.Implementations
{
    public class BlogRoleValidation : IBlogRoleValidation
    {
        private IBlogOwnersStore<BlogOwner> _blogOwnersStore;
        private IBlogEditorsStore<Blog, ApplicationUser> _blogEditorStore;
        public BlogRoleValidation(IBlogOwnersStore<BlogOwner> blogOwnersStore,
                                  IBlogEditorsStore<Blog, ApplicationUser> blogEditorStore)
        {
            _blogOwnersStore = blogOwnersStore;
            _blogEditorStore = blogEditorStore;
        }

        public Task<bool> ValidateEditor(ApplicationUser user, Blog blog)
        {
            return _blogEditorStore.IsEditor(blog, user);
        }

        public Task<bool> ValidateOwner(ApplicationUser user, Blog blog)
        {
            return _blogOwnersStore.IsOwner(user.Id, blog.Id);
        }
    }
}
