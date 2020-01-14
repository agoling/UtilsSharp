**UtilsSharp工具类库简介**：该工具类是基于dotnet framework 4.5.2封装的，里面包括：小驼峰法命名出参规范类、出入参规范类、阿里oss帮助类、钉钉机器人帮助类、图片帮助类、文件帮助类、下载类、随机数帮助类、对象映射帮助类、字符串帮助类、验证码生成、时间帮助类、中国日历帮助类、任务下发帮助类等，后面将持续更新中…

例如：UtilsCore.Result目录，该目录下的类为出入参规范类，其使用方法如下："BaseResult"类前后端接口返回参命名规范
 
### 一、命名规范

#### 1、返回参小驼峰法命名：除第一个单词之外，其他单词首字母大写

①Mvc 引入：MvcHelper 
```
public JsonResult GetInfo(){
   BaseResult<User> result=new BaseResult<User>();
  //此方法返回给前端可以转为小驼峰法命名
  return new JsonFormatResult(result, JsonRequestBehavior.AllowGet);
}

```
②webApi 引入：WebApiHelper
在Global.asax.cs类的Application_Start方法内

```
 public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //返回参小驼峰法命名
            JsonFormatConfig.Register(GlobalConfiguration.Configuration);
        }
    }
```

```
public JsonResult GetInfo(){
   //配置后，方法内自动会转为小驼峰法命名
   BaseResult<User> result=new BaseResult<User>();
   return Json(result, JsonRequestBehavior.AllowGet);;
}
```


#### 2、方法命名规范
```
    获取单个对象的方法用get作为前缀。
    获取多个对象方法用search作为前缀。
    删除的方法用delete作为前缀。
    (添加+修改)用save作为前缀。
```
#### 3、变量命名规范
##### 变量取名应见名识意,尽量用英文单词,而不是缩写.
```
example：pageIndex, pageSize
```

### 二、接口返回格式

```
{
    code:200,       //数字
    msg:"success",  //成功时为"success"，错误时则是错误信息
    result:{},        //对象
}
```
### 三、操作类返回格式

##### 增删改都返回对应ID，查询返回完整模型
```
{
    code:200,       //数字
    msg:"success", //字符串
    result:{},
}
```

### 四、列表接口基础出入参

##### 请求的基础参数
```
{
    keyword:"",    //关键词搜索
    pageIndex:1,   //当前页码
    pageSize:10,   //分页数量
}
```

##### 返回的基础内容
```
{
    code:200, 
    msg:"success",
    result:{
        list:[{},{}],  //返回的列表
        pageIndex:1,   //当前页码
        pageSize:10,   //分页数量
        totalCount:100,//总条数
        totalPages:10  //总页数 
        
    },
}
```
### 五、错误码

返回码 | 标识 |  说明  
-|-|-
200 | success | 请求成功 |
999 | defaultError | 系统繁忙，此时请开发者稍候再试 |
3000 | nullData | 未找到数据 |
4000 | notLogin | 未登录 |
5000 | exception | 异常 |
5010 | dataIsValid |  数据验证不通过 |
6000 | dataExpire | 数据过期 |
7000 | businessError | 默认业务性异常 |
