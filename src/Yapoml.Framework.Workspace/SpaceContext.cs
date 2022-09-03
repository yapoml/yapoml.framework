﻿using System.Collections.Generic;

namespace Yapoml.Framework.Workspace
{
    public class SpaceContext
    {
        public SpaceContext(string name, WorkspaceContext workspace, SpaceContext parentSpaceContext)
        {
            Workspace = workspace;
            ParentSpace = parentSpaceContext;

            _name = name;
        }

        private string _name;
        private string _normalizedName;
        public string Name
        {
            get
            {
                if (_normalizedName is null)
                {
                    _normalizedName = Workspace.NameNormalizer.Normalize(_name); ;
                }

                return _normalizedName;
            }
        }

        private string _namespace;
        public string Namespace
        {
            get
            {
                if (_namespace is null)
                {
                    if (ParentSpace != null)
                    {
                        _namespace = $"{ParentSpace.Namespace}.{Name}";
                    }
                    else
                    {
                        _namespace = $"{Workspace.RootNamespace}.{Name}";
                    }
                }

                return _namespace;
            }
        }

        public WorkspaceContext Workspace { get; }

        public SpaceContext ParentSpace { get; }

        public IList<SpaceContext> Spaces { get; } = new List<SpaceContext>();

        public IList<PageContext> Pages { get; } = new List<PageContext>();

        public IList<ComponentContext> Components { get; } = new List<ComponentContext>();
    }
}
