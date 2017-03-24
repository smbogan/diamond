using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class Repository : IDisposable
    {
        LibGit2Sharp.Repository repository;

        string location;

        string name;
        string email;



        public Repository(string location, string name, string email)
        {
            this.name = name;
            this.email = email;
            this.location = location;

            if(LibGit2Sharp.Repository.IsValid(location))
            {
                repository = new LibGit2Sharp.Repository(location);
            }
            else
            {
                Directory.CreateDirectory(location);
                LibGit2Sharp.Repository.Init(location);
                repository = new LibGit2Sharp.Repository(location);
            }
            
        }

        public bool Exists(ResourceIdentifier identifier)
        {
            return File.Exists(Path.Combine(location, identifier.Identifier));
        }

        public Stream ReadFile(string url)
        {
            return File.OpenRead(Path.Combine(location, url));
        }

        public Stream ReadFile(ResourceIdentifier identifier)
        {
            return File.OpenRead(Path.Combine(location, identifier.Identifier));
        }

        public void WriteFile(string url, Stream stream)
        {
            var dest = Path.Combine(location, url);

            Directory.CreateDirectory(Path.GetDirectoryName(dest));

            using (var fs = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                stream.CopyTo(fs);
            }

            LibGit2Sharp.Commands.Stage(repository, url);
        }

        public void WriteFile(ResourceIdentifier identifier, Stream stream)
        {
            var dest = Path.Combine(location, identifier.Identifier);

            Directory.CreateDirectory(Path.GetDirectoryName(dest));

            using (var fs = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                stream.CopyTo(fs);
            }

            LibGit2Sharp.Commands.Stage(repository, identifier.Identifier);
        }

        public void Commit(string message = "none")
        {
            var signature = new LibGit2Sharp.Signature(name, email, DateTimeOffset.Now);

            try
            {
                var result = repository.Commit(message, signature, signature, new LibGit2Sharp.CommitOptions()
                {
                    AllowEmptyCommit = false,
                    PrettifyMessage = false,
                    AmendPreviousCommit = false
                });
            }
            catch(EmptyCommitException e)
            {
                //Ignore empty commits
            }
        }

        public void Undo()
        {
            repository.Reset(LibGit2Sharp.ResetMode.Hard, repository.Head.Commits.ElementAt(0));
        }

        public void Dispose()
        {
            repository.Dispose();
        }
    }
}
