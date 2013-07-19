using System.Collections.Generic;
using System.Linq;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVDatabase.Entities.Enums;

namespace Mediaportal.TV.Server.TVDatabase.EntityModel.Interfaces
{
  public interface IChannelRepository : IRepository<TvModel>
  {
    IQueryable<Channel> GetAllChannelsByGroupIdAndMediaType(int groupId, MediaTypeEnum mediaType);
    IQueryable<Channel> GetAllChannelsByGroupId(int groupId);
    IQueryable<Channel> IncludeAllRelations(IQueryable<Channel> query);
    IQueryable<Channel> IncludeAllRelations(IQueryable<Channel> query, ChannelIncludeRelationEnum includeRelations);
    IQueryable<TuningDetail> IncludeAllRelations(IQueryable<TuningDetail> query);
    IQueryable<ChannelMap> IncludeAllRelations(IQueryable<ChannelMap> query);
    IList<Channel> LoadNavigationProperties(IEnumerable<Channel> channels, ChannelIncludeRelationEnum includeRelations);
    Channel LoadNavigationProperties(Channel channel, ChannelIncludeRelationEnum includeRelations);
    Channel LoadNavigationProperties(Channel channel);
    IList<Channel> LoadNavigationProperties(IEnumerable<Channel> channels);
  }
}