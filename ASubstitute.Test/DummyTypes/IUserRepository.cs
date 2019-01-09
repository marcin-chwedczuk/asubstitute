using System.Collections.Generic;

namespace ASubstitute.Test.DummyTypes {
    public interface IUserRepository {
        User FindById(int userId);
        ICollection<User> FindAll();

        void Add(User user);
        void Delete(User user);
        void Update(User user);
    }
}
