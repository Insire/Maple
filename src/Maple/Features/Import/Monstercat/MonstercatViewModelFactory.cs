using MvvmScarletToolkit;
using SoftThorn.MonstercatNet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maple
{
    public sealed class MonstercatViewModelFactory : IMonstercatViewModelFactory
    {
        private readonly IMonstercatApi _monstercatApi;
        private readonly IScarletCommandBuilder _commandBuilder;

        public MonstercatViewModelFactory(IMonstercatApi monstercatApi, IScarletCommandBuilder commandBuilder)
        {
            _monstercatApi = monstercatApi ?? throw new ArgumentNullException(nameof(monstercatApi));
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
        }

        public async Task<IMonstercatViewModel> Create(string input, CancellationToken token)
        {
            if (!input.Contains("monstercat.com", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            if (Uri.TryCreate(input.Trim(), UriKind.RelativeOrAbsolute, out var url))
            {
                if (url.Segments.Length != 3)
                {
                    return null;
                }

                switch (url.Segments[1].Trim('/').ToLowerInvariant())
                {
                    case "release":
                        {
                            var id = url.Segments[2];
                            var result = await _monstercatApi.GetRelease(id);
                            if (result?.Release is null)
                            {
                                return null;
                            }

                            return new MonstercatReleaseViewModel(_monstercatApi, _commandBuilder, result.Release);
                        }

                    case "playlist":
                        {
                            var id = url.Segments[2];
                            if (!Guid.TryParse(id, out var guid))
                            {
                                return null;
                            }

                            return new MonstercatPlaylistViewModel(_monstercatApi, _commandBuilder, new SoftThorn.MonstercatNet.Playlist()
                            {
                                // TODO fetch and add playlist information
                                // also fetch all track meta data
                                Id = guid,
                                Name = "Test",
                            });
                        }
                }
            }

            return null;
        }
    }
}
