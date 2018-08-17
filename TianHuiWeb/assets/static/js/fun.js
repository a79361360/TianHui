$(function(){
	if(!!document.body.addEventListener){
		document.body.addEventListener('touchstart', function() {}, false);
	}
	var scrollTop=0;
	$(window).scroll(function(event) {
		scrollTop=$(window).scrollTop();
		if(scrollTop>50){
			$('.header').addClass('cur');
		}else{
			$('.header').removeClass('cur');
		}
	});

	//手机导航
	$('.menu-btn').bind('click', function(){
		$('.menu').addClass('cur');
		$('.maskm').fadeTo(100,0.7);
	});
	$('.maskm').bind('touchstart', function(){
		$('.menu').removeClass('cur');
		$('.maskm').fadeOut(150);
	});
	$('.menu,.maskm').bind('touchmove', function(e){
		e.preventDefault();
	});

	//
	Reset();
	$(window).resize(Reset);
	function Reset(){
		var winWidth = $(window).width();

		if(winWidth>768){
			$('.gupiao').height($('.gupiao').width()*545/1096);
			if($('.middle').length>0){
				var midImgHeight = $('.middle .img').height();
				$('.middle .text').height(midImgHeight);
			}
		}else{
			$('.banner .img').height(winWidth*320/640);
			$('.banner .img img').css({'width':winWidth*32/15, 'height': winWidth*320/640, 'margin-left': -winWidth*8/15});
			//$('.product-banner .img .m').css({'width':503, 'height': 178, 'margin-left': -(503-winWidth)/2});

			if($('#subMenu-tit').length>0){
				$('.container,.product-banner').css({'padding-top':'0'});
				$(window).scroll(function(){
					var _Top = $(window).scrollTop();
					if(_Top>44){
						$('body').css({'margin-top':'-44px'});
						$('.header').css({'top':'-44px'});
						$('#subMenu-tit').addClass('cur');
						$('.container').css({'padding-top':'154px'});
					}else{
						$('body').css({'margin-top':'0'});
						$('.header').css({'top':'0'});
						$('#subMenu-tit').removeClass('cur');
						$('.container').css({'padding-top':'0'});
					}
				});
			}
			$('#subMenu-tit').click(function(){
				$('.fixedbarss').addClass('cur');
			});
			$('.fixedbarss .close').click(function(){
				$('.fixedbarss').removeClass('cur');
			});
			$('.fixedbarss').bind('touchmove', function(e){
				e.preventDefault();
			});
		}
		if(winWidth>800){
			if(!$('.menu').data('state')){
				$('.menu').data('state',true);
				$('.menu').data('state1',false);
				$('.menu ul li >a').unbind('click');
				$('.menu ul li').mouseover(function(){
					$(this).children('dl').stop(true, true).slideDown(150);
				}).mouseleave(function(){
					$(this).children('dl').stop(true, true).slideUp(150);
				});
			}
		}else{
			if(!$('.menu').data('state1')){
				$('.menu').data('state',false);
				$('.menu').data('state1',true);
				$('.menu ul li').unbind('mouseover mouseleave');
				// $('.menu ul li >a').click(function(e){
				// 	if($(this).next().children('dd').length>0){
				// 		e.preventDefault();
				// 		$(this).next('dl').stop(true, true).slideToggle(150).parent().siblings().children('dl').stop(true, true).slideUp(150);
				// 	}
				// });
			}
		}
	}
});

function cutimg(imgObj, par) {
	var img_w = $(imgObj).attr('width'),
		img_h = $(imgObj).attr('height'),
		parent_w = $(imgObj).parent().width(),
		parent_h = $(imgObj).parent().height();

	if(par!='' && par!=null){
		parent_w = $(par).width();
		parent_h = $(par).height();
	}
	if(img_w==undefined || img_h==undefined){
		img_w = $(imgObj).width();
		img_h = $(imgObj).height();
	}

	if ((img_w/img_h) > (parent_w/parent_h)) {
		img_w = (img_w/img_h)*parent_h;
		img_h = parent_h;
		var left = ((img_w - parent_w) / 2)*-1;
		$(imgObj).css({"display":"block","margin-top":"0px","margin-left":left + "px","width":img_w,"height":img_h});
	} else {
		img_h = (img_h/img_w)*parent_w;
		img_w = parent_w;
		var top = ((img_h - parent_h) / 2)*-1;
		$(imgObj).css({"display":"block","margin-top":top + "px","margin-left":"0px","width":img_w,"height":img_h});
	}
}

function BackTop(){
	$(window).scroll( function() {
		var scrollValue=$(window).scrollTop();
		scrollValue > 100 ? $('.BackTop').fadeIn():$('.BackTop').fadeOut();
	} );
	$('.BackTop').click(function(){
		$("html,body").animate({scrollTop:0},200);
	});
}

function slide(obj,delay,speed){
	var theInt = null;
	var i=p=0;
	var html="";
	var $imgs=$(obj).find("ul");
	var num=$imgs.find("li").size();
	var state=true;
	for(i=0;i<num;i++){
		html+=("<span></span>");
	}
	$('.pagina').html(html);
	var $ctrs=$(obj).find(".pagina");

	//点击切换
	$ctrs.find("span").each(function(i){
		$(this).click(function(){
			if(!$(this).hasClass("cur")){
				slide_state(i);
			}
		});
	});
	$('.arrow-left').click(function(){
		if(state){
			state=false;
			p--;
			if(p<0){
				p=num-1;
			}
			slide_state(p);
		}
	});
	$('.arrow-right').click(function(){
		if(state){
			state=false;
			p++;
			if(p>=num){
				p=0;
			}
			slide_state(p);
		}
	});
	//自动播放
	auto_play=function(p){
		clearTimeout(theInt);
		theInt=setTimeout(function(){
			p++;
			if(p>=num){
				p=0;
			}
			slide_state(p);
		},delay);
	}
	//当前状态
	slide_state=function(num){
		$imgs.find("li").fadeOut(speed).removeClass('cur').eq(num).fadeIn(speed,function(){$(this).addClass('cur');state=true});
		$ctrs.find("span").removeClass("cur").eq(num).addClass("cur");
		auto_play(num);
	}
	//初始化
	slide_state(p);
}

var init={
	pc: function (){
		slide(".imgs",5000,1000);
		Reset();
		$(window).resize(function(){
			Reset();
		});
		function Reset(){
			$('.imgs ul img').each(function(){
				cutimg(this, '.imgs ul');
			});
			$('.wrap a img').each(function(){
				cutimg(this);
			});
		}

		$('.wrap ul li a').hover(function(){
			$(this).addClass('hover');
		},function(){
			$(this).removeClass('hover');
		});
	},
	mobile: function (){
		var mySwiper = new Swiper('.swiper-container', {
			pagination: '.pagination',
			paginationClickable: true,
			spaceBetween: 30,
			centeredSlides: true,
			autoplayDisableOnInteraction: false,
			autoplay: 5000
		});
	}
}

var commonFun = {
	tab: function(str){
		var container = $(str.container),
			trigger = container.find(str.trigger),
			cont = container.find(str.cont);
		$.each(trigger, function (index) {
			$(this).click(function(){
				trigger.removeClass('cur').eq(index).addClass('cur');
				cont.hide().eq(index).show();
			});
		});
	},
	grid: function(){
		var gridCont = $('.module-grid').eq(1).children('.module-grid-cont');
		var gridContHeight = gridCont.height();
		$('.more').click(function(){
			$(this).toggleClass('cur').not('.cur').html('展开更多信息<span class="more-icon"><i class="icon icon-down-g"></i></span>').end().filter('.cur').html('收起<span class="more-icon"><i class="icon icon-down-g"></i></span>')
			if(!$(this).hasClass('cur')){
				$('html,body').animate({scrollTop: $(window).scrollTop()-gridCont.height()+gridContHeight},300);
				if($(window).width()>768){
					gridCont.animate({height: '172px'},300);
				}else{
					gridCont.animate({height: '148px'},300);
				}
			}else{
				if($(window).width()>768){
					gridCont.animate({height: '860px'},300);
				}else{
					gridCont.animate({height: '740px'},300);
				}
			}
		});
	},
	slide: function(options){
		var $container = $(options.container),
			$trigger = $container.find(options.trigger),
			$cont = $container.find(options.cont);
		speed = options.speed || 4000,
			state = true, n=0, num=$trigger.last().index(), t='';

		$trigger.click(function(){
			if(state){
				n = $(this).index();
				fade(n);
				autoPlay();
			}
		});
		function fade(n){
			state=false;
			$cont.stop(true, true).fadeOut(1000).eq(n).fadeIn(1000, function(){state=true});
			$trigger.removeClass('cur').eq(n).addClass('cur');
		}
		function autoPlay(){
			clearInterval(t);
			t = setInterval(function(){
				n = n+1>num ? 0 : n+1;
				fade(n);
			}, speed);
		}
		autoPlay();
	}
}

function Alert(err, id) {
	var str = '<div class="alert"><div class="hd">系统提示</div><span class="close"></span><div class="cont">'+ err +'</div><div class="button"><a class="act" href="javascript:;">确定</a></div></div>';
	$('<div class="mask"></div>').appendTo('body').fadeIn(300, function () {
		$('body').append(str);
	});
	$('body').one('click', '.alert a,.alert .close', function () {
		$('.mask,.alert').fadeOut(300, function () {
			$(this).remove();
			if (id!='' && id!=null) {
				$(id).val('').focus();
			}
		});
	});
	return;
}

function selectUl(box, ul) {
	$('body').bind('click', function (event) {
		$(box).each(function () {
			$(this).removeClass('cur').children(ul).slideUp(150);
		});
	});
	$(box).click(function (event) {
		event.stopPropagation();
		if (!$(this).hasClass('disabled')) {
			$(this).addClass('cur').children(ul).slideDown(150);
			$(box + '.cur').not(this).each(function () {
				$(this).removeClass('cur').children(ul).slideUp(150);
			});
		}
	});
	$(ul).delegate('li', 'click', function (event) {
		event.stopPropagation();
		$(this).addClass('cur').siblings().removeClass('cur');

		// 获取城市
		var _provinceId = $(this).attr('data-id');
		var _provinceName = $(this).html();
		if ($(this).attr('data-level') == 1) {
			var that = this;
			$('#city').val('请选择');
			$('#cityList').parent().removeClass('disabled');
			$.post('/api/area/findCompanyArea', {'area': _provinceId}, function (data) {
				switch (data.errcode) {
					case 0:
						var _str = '';
						if (data.rs && data.rs.length > 0) {
							for (var k in data.rs) {
								var _v = data.rs[k];
								_str += ' <li data-id="' + _v.area_id + '" data-level="' + _v.level + '">' + _v.area_name + '</li>'
							}
							$('#cityList').parent().removeClass('disabled');
							$('#cityList').html(_str);
						} else {
							_str += ' <li data-id="0" data-level="2">' + _provinceName + '</li>'
							$('#cityList').html(_str);
							$('#cityList li:first').click().parent().parent().addClass('disabled');
						}
						break;
					default :
						Alert('系统繁忙！');
						break;
				}
			}, 'json');
		}

		$(this).parent().prevAll('input').val($(this).text());
		$(box + '.cur').each(function () {
			$(this).removeClass('cur').children(ul).slideUp(150);
		});
	});
}

function caselist(){
	var container = $('.J-caselist'),
		page = container.find('.J-caselist-page'),
		pageItem = page.find('li'),
		pageNav = container.find('.J-caselist-pagenav'),
		cont = container.find('.J-caselist-cont'),
		prev = container.find('.J-page-prev'),
		next = container.find('.J-page-next');
	var winW = $(window).width();
	var pageItemW = pageItem.eq(0).outerWidth();


	page.css({
		width : (pageItem.length * pageItemW) + 'px',
	});

	loadMoreCase(cont.eq(0));
	for(var i = 0; i < pageItem.length; i++){
		pageNav.append('<li><span></span></li>');
		cont.eq(i).addClass('J-caselist-cont' + (i + 1));
		loadMoreCase('.J-caselist-cont' + (i + 1));
	}

	if((pageItemW * pageItem.length) <= page.parent().width()){
		prev.hide();
		next.hide();
	}

	var pageNavItem = pageNav.find('li');
	pageNavItem.eq(0).addClass('cur');

	$.each(pageItem, function(index){
		$(this).click(function(){
			pageItem.removeClass('cur').eq(index).addClass('cur');
			pageNavItem.removeClass('cur').eq(index).addClass('cur');
			cont.hide().eq(index).show();
		});
	});
	$.each(pageNavItem, function(index){
		$(this).click(function(){
			pageItem.removeClass('cur').eq(index).addClass('cur');
			pageNavItem.removeClass('cur').eq(index).addClass('cur');
			cont.hide().eq(index).show();
			var left = -parseInt(page.css('left')) / pageItemW,
				limitNum = page.parent().width() / pageItemW;

			if(index < left){
				page.css({
					left: -index * pageItemW + 'px'
				});
			}
			if((index + 1) > (limitNum + left)){
				page.css({
					left: -(index + 1 - left - limitNum) * pageItemW + 'px'
				});
			}
			if((index + 1) == (limitNum + left)){
				page.css({
					left: (index - left - limitNum) * pageItemW + 'px'
				});
			}
		});
	});
	var statue = true,
		time = 500;
	prev.click(function(){
		if(statue == true){
			statue = false;
			setTimeout(function(){
				statue = true;
			}, time);
			var left = parseInt(page.css('left'));
			if(left == 0){
				return false;
			}
			page.css({
				left: left + pageItemW + 'px',
			});
		}
		else{
			return false;
		}
	});
	next.click(function(){
		if(statue == true){
			statue = false;
			setTimeout(function(){
				statue = true;
			}, time);
			var left = parseInt(page.css('left'));
			if(page.width() <= ((page.parent().width()) - left)){
				return false;
			}
			page.css({
				left: left - pageItemW + 'px',
			});
		}
		else{
			return false;
		}
	});
	function loadMoreCase(target1){
		var target = $(target1),
			more = target.find('.J-list-more');
		var categoryId = more.attr('data-id'),
			num = parseInt(more.attr('data-num'));
		var type = more.attr('data-type');
		more.unbind('click').bind('click',function(){
			$.post('/api/archives/findMoreCases', {'categoryId': categoryId, 'limit' : num}, function(data){
					if(data.rs.length < 8){
						more.hide();
					}
					more.attr('data-num', (num + 8));
					for(var i = 0; i < data.rs.length; i++){
						if (type == 'frame') {
							target.find('ul').append('<li class="caselist-item caselist-item'+ ((i + 1) % 4) + '">'+
								'<a href="/casedetail/'+ data.rs[i].archives_id +'">'+
								'<span class="item-img">'+
								'<img src="/uploads/pics/s/'+ data.rs[i].cover +'">'+
								'</span>'+
								'<span class="item-title">' + data.rs[i].title + data.rs[i].win+ '</span>'+
								'</a></li>');
						} else {
							target.find('ul').append('<li class="caselist-item caselist-item'+ ((i + 1) % 4) + '">'+
								'<a href="/casedetail/'+ data.rs[i].archives_id +'">'+
								'<span class="item-img">'+
								'<img src="/uploads/pics/s/'+ data.rs[i].cover +'">'+
								'<span class="item-mask">'+
								'<img src="/assets/static/images/case/caselist-item-video.png">'+
								'</span>'+
								'</span>'+
								'<span class="item-title">' + data.rs[i].title + data.rs[i].win+ '</span>'+
								'</a></li>');
						}
					}
					console.log(1);
				}, 'json'
			)
		});
	}
}

function setPoster(){
	var winW = $(window).width();
	if(winW > 1024){
		$('video').removeAttr('poster');
	}
}




