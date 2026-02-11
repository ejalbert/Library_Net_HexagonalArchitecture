using System;
using System.Reflection;

namespace LibraryManagement.Api.Rest.Legacy.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}