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
            CreateMap<UpdateWorkRequest, WorkEntity>();
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

            CreateMap<UserEntity, UserInfoResponse>();

            CreateMap<ApplyRequest, ApplyEntity>();
            CreateMap<ApplyEntity, ApplyResponse>();

            CreateMap<MessageStatusRequest, MessageEntity>();
            CreateMap<MessageRequest, MessageEntity>();
            CreateMap<MessageEntity, MessageResponse>();

            CreateMap<AnnounceStatusRequest, AnnounceEntity>();
            CreateMap<AnnounceRequest, AnnounceEntity>();
            CreateMap<AnnounceEntity, AnnounceResponse>();

        }
    }
}
