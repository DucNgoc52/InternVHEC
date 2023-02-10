using IMS_LEARN.Common;
using IMS_LEARN.Models;
using IMS_LEARN.Services.SvStaff;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using IMS_LEARN.Models.Paging;
using IMS_LEARN.Infratructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using IMS_LEARN.Services.Token;
using IMS_LEARN.Models.StaffModels;

namespace IMS_LEARN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly ImsDbContext _context;
        private readonly AppSetting _appSettings;
        //private readonly IEmailSender _emailSender;
        public IConfiguration _configuration { get; set; }
        private readonly ITokenService _tokenService;

        public StaffController(IStaffService staffService, ImsDbContext context, IConfiguration configuration, IOptionsMonitor<AppSetting> optionsMonitor, ITokenService tokenService)
        {
            _staffService = staffService;
            _context = context;
            _configuration = configuration;
            _appSettings = optionsMonitor.CurrentValue;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Get List Staff by Paging or All
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        [Route("GetList")]
        public IActionResult GetList(ResquestParameters parameters)
        {
            try
            {
                var tempStaffs = _staffService.GetList();

                // Search
                if (!string.IsNullOrEmpty(parameters.Filter))
                {
                    var optionAssembly = ScriptOptions.Default.AddReferences(typeof(StaffModel).Assembly);
                    var tempFilterExpression = CSharpScript.EvaluateAsync<Func<StaffModel, bool>>($"s=> {parameters.Filter}", optionAssembly);
                    Func<StaffModel, bool> filterExpression = tempFilterExpression.Result;

                    tempStaffs = tempStaffs.Where(filterExpression).AsQueryable();
                }

                // Order by
                if (!string.IsNullOrEmpty(parameters.OrderBy))
                {
                    tempStaffs = tempStaffs.OrderBy(parameters.OrderBy);
                }
                else
                {
                    tempStaffs = tempStaffs.OrderBy(x => x.StaffCode);
                }

                // Check dropdown
                if (parameters.IsDropdown)
                {
                    var tempStaffsDropdown = PagedList<StaffModel>.ToPagedList(tempStaffs.ToList(), 0, tempStaffs.Count());
                    return Ok(new StaffListModel { Items = tempStaffsDropdown });
                }

                int totolCount = tempStaffs.Count();
                //int skip = parameters.Skip != null ? parameters.Skip.Value : 0;
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = tempStaffs.Skip(skip).Take(top).ToList();
                var results = new PagedList<StaffModel>(items, totolCount, (skip / top) + 1, top);

                return Ok(new StaffListModel { Items = results, MetaData = results.MetaData });

            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel()
                {
                    Code = 1,
                    IsSuccess = false,
                    Message = $"Something went wrong! {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get Staff by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByCode/{code}")]
        public IActionResult GetByCode(string code)
        {
            try
            {
                var data = _staffService.GetByCode(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel()
                {
                    Code = 1,
                    IsSuccess = false,
                    Message = $"Something went wrong! {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(StaffModel input)
        {
            try
            {
                var existStaff = _staffService.GetByCode(input.StaffCode);
                if (existStaff == null)
                {
                    var staff = _staffService.Create(input);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Add Staff Successfully!"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Add Staff Failure!"
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Staff code is exist."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Add Staff Failure {ex.Message.ToString()}"
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(StaffModel input)
        {
            try
            {
                var existStaff = _staffService.GetByCode(input.StaffCode);
                if (existStaff != null)
                {
                    var staff = _staffService.Update(input);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Update staff successfully!"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Update staff failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Staff code is exist."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Update failure {ex.Message.ToString()}"
                });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(string staffcode)
        {
            try
            {
                var existStaff = _staffService.GetByCode(staffcode);
                if (existStaff != null)
                {
                    var staff = _staffService.Delete(staffcode);
                    if (staff != null)
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = true,
                            Code = 0,
                            Message = "Delete staff successfully"
                        });
                    }
                    else
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 1,
                            Message = "Delete staff failure."
                        });
                    }
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "Staff code is exist."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = $"Delete staff failure. {ex.Message.ToString()}"
                });
            }
        }

        // băm mật khẩu md5
        private string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel login)
        {
            var exitsPermits = _context.Staffs.SingleOrDefault(p => p.UserName == login.UserName && CreateMD5(login.Password) == p.PassWord);
            if (exitsPermits != null)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 0,
                    Message = "Login successfully",
                    Data = CreateToken(exitsPermits)
                });
            }
            else
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = "Account or password incorrect"
                });
            }
        }

        private TokenModel CreateToken(Staff staff)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, staff.FirtName),
                    new Claim(JwtRegisteredClaimNames.Email, staff.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", staff.UserName),
                    new Claim("Id", staff.Id.ToString()),

                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                StaffCode = staff.StaffCode,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddSeconds(30)
            };

            _context.Add(refreshTokenEntity);
            _context.SaveChanges();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private string GenerateRefreshToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new BaseResultModel
                        {
                            IsSuccess = false,
                            Code = 0,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 0,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 0,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 0,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 0,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 0,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = _context.Staffs.SingleOrDefault(s => s.StaffCode == storedToken.StaffCode);
                var token = CreateToken(user);

                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 1,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch (Exception)
            {
                return BadRequest(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 0,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }

        [HttpPut]
        [Route("ChangePassWord")]
        public IActionResult ChangePass(ChangePassword request)
        {
            var user = _context.Staffs.SingleOrDefault(p => p.StaffCode == request.StaffCode);
            if (user == null)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 0,
                    Message = "StaffCode is not exist!"
                });
            }
            else
            {
                var pass = _context.Staffs.FirstOrDefault(p => p.PassWord == CreateMD5(request.CurrentPassword));
                if (pass == null)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 0,
                        Message = "Current Password incorrect "
                    });
                }
                else
                {
                    _staffService.ChangePass(request);
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = true,
                        Code = 1,
                        Message = "Change password successfully "
                    });
                }
            }
        }
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout(string staffcode)
        {
            var token = _context.RefreshTokens.FirstOrDefault(p => p.StaffCode == staffcode);
            if (token != null)
            {
                _tokenService.Logout(staffcode);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 0,
                    Message = "Logout successfully "
                });
            }
            else
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 0,
                    Message = "Logout failure"
                });
            }
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordModel forgot)
        {
            var user = _context.Staffs.SingleOrDefault(p => p.Email == forgot.Email);
            if (user != null)
            {
                _staffService.ForgotPass(forgot);
                var subject = "Reset Password";
                var body = $"Hi {user.LastName}!  Your password reset code : {user.ResetPasswordCode} ";
                SendEmail(user.Email, body, subject);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 0,
                    Message = "Please check your email"
                });
            }
            else
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = "Email is not exist"
                });
            }
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("ngocrobot52@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("fbiwaring00@gmail.com", "lpnzlmcphcqgrhgf");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword input)
        {
            var user = _context.Staffs.SingleOrDefault(p => p.ResetPasswordCode == input.TokenResetPassword);
            if( user != null)
            {
                if ( input.NewPassword != input.ConfirmPassword)
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 1,
                        Message = "New password and confirm password don't coincide."
                    });
                }
                else
                {
                    _staffService.ResetPass(input);
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = true,
                        Code = 0,
                        Message = "Reset password successfully"
                    });
                }
            }
            else
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 1,
                    Message = "Password reset code not exactly"
                });
            }
        }

        [HttpGet]
        [Route("Timkiemtheoten")]
        public IActionResult Timkiemtheoten(string name)
        {
            var data = _staffService.Timkiem(name);
            return Ok(data);
        }
    }
}
