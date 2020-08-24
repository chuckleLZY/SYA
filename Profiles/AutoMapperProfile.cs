using AutoMapper;
using SyaApi.Entities;
using SyaApi.Requests;
using SyaApi.Responses;

namespace SyaApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<RegisterRequest, AccountEntity>();
            CreateMap<AccountEntity, AccountResponse>();
            
            CreateMap<ResumeRequest, ResumeEntity>();
            CreateMap<ResumeEntity, ResumeResponse>();
            CreateMap<CommentRequest, CommentEntity>();
            CreateMap<CommentEntity, CommentResponse>();
            CreateMap<AnnounceRequest, AnnounceEntity>();
            CreateMap<AnnounceEntity, AnnounceResponse>();
            CreateMap<FavoriteRequest, FavoriteEntity>();
            CreateMap<FavoriteEntity, FavoriteResponse>();
            CreateMap<HistoryRequest, HistoryEntity>();
            CreateMap<HistoryEntity, HistoryResponse>();

        }
    }
}
