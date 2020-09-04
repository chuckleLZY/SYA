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

            CreateMap<ProvideWorkRequest, WorkEntity>();
            CreateMap<WorkEntity, WorkResponse>();

            CreateMap<LeaveRequest, LeaveEntity>();
            CreateMap<LeaveEntity, LeaveResponse>();

            CreateMap<UserRequest, UserEntity>();
            CreateMap<UserEntity, UserResponse>();
            CreateMap<CommentRequest, CommentEntity>();
            CreateMap<CommentEntity, CommentResponse>();
            CreateMap<AnnounceRequest, AnnounceEntity>();
            CreateMap<AnnounceEntity, AnnounceResponse>();

            CreateMap<HistoryRequest, HistoryEntity>();
            CreateMap<HistoryEntity, HistoryResponse>();
            CreateMap<ViewHistoryRequest, HistoryItemEntity>();
            CreateMap<HistoryItemEntity, HistoryResponse>();


            CreateMap<UserEntity, UserInfoResponse>();

            CreateMap<ApplyRequest, ApplyEntity>();
            CreateMap<ApplyEntity, ApplyResponse>();

            CreateMap<FavoriteRequest, FavoriteEntity>();
            CreateMap<FavoriteEntity, FavoriteResponse>();
            CreateMap<CFavoriteRequest, FavoriteItemEntity>();
            CreateMap<FavoriteItemEntity, FavoriteResponse>();
            CreateMap<CrFavoriteRequest, FavoriteEntity>();
            CreateMap<FavoriteEntity, FavoriteResponse>();

            CreateMap<FhasworkRequest, FhasworkEntity>();
            CreateMap<FhasworkEntity, FhasworkResponse>();
            CreateMap<ViewRequest, FhasworkItemEntity>();
            CreateMap<FhasworkItemEntity, FhasworkItemResponse>();
            


        }
    }
}
