using AutoMapper;
using EPassBook.DAL.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EPassBook.Controllers
{
    public class APIWorkFlowController : ApiController
    {
        private readonly IMapper _mapper;
        IUserService _userService;
        ICommentService _icommentService;

        public APIWorkFlowController(IUserService userService, IMapper mapper, ICommentService commentService)
        {
            _icommentService = commentService;
            _userService = userService;
            _mapper = mapper;
        }


    }
}
