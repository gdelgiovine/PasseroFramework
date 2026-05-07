using System;
using System.Collections.Generic;
namespace Passero.Framework.HttpHelper
{
    public sealed class PreWisejCommandRouter
    {
        private readonly Dictionary<string, IPreWisejCommandHandler> _handlers;

        public PreWisejCommandRouter(
            IDictionary<string, IPreWisejCommandHandler> handlers = null)
        {
            _handlers = new Dictionary<string, IPreWisejCommandHandler>(
                StringComparer.OrdinalIgnoreCase)
            {
                ["PING"] = new PingPreWisejCommandHandler()
            };

            if (handlers == null)
                return;

            foreach (var handler in handlers)
                _handlers[handler.Key] = handler.Value;
        }

        public bool TryGetDefinition(string path, out PreWisejCommandDefinition definition)
        {
            definition = null;

            if (IsWisejPath(path))
                return false;

            var command = GetCommand(path);
            if (string.IsNullOrEmpty(command))
                return false;

            if (!_handlers.TryGetValue(command, out var handler))
                return false;

            definition = handler.Definition;
            return true;
        }

        public bool TryHandle(
            PreWisejRequestData request,
            out PreWisejCommandResult result)
        {
            result = new PreWisejCommandResult();

            var command = GetCommand(request.Path);
            if (string.IsNullOrEmpty(command))
                return false;

            if (!_handlers.TryGetValue(command, out var handler))
                return false;

            return handler.TryHandle(request, out result);
        }

        private static bool IsWisejPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return path.StartsWith("/app.wx", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/wisej.wx", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/ws.wx", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/resource.wx", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetCommand(string path)
        {
            var normalizedPath = (path ?? string.Empty).Trim('/');

            if (string.IsNullOrEmpty(normalizedPath))
                return string.Empty;

            var slashIndex = normalizedPath.IndexOf('/');
            return slashIndex < 0
                ? normalizedPath
                : normalizedPath.Substring(0, slashIndex);
        }
    }
}