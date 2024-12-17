
using AutoMapper;
using TechShop.Models;
using TechShop.ViewModel;

namespace TechShop.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() { // chuyển từ kiểu registerVM sang user
            CreateMap<RegisterVM, User>();
            //.ForMember(customer => customer.Name, option => option.MapFrom(RegisterVM => RegisterVM.Name))
            //.ReverseMap();// chuyen doi 2 chieu
        }
    }
}
