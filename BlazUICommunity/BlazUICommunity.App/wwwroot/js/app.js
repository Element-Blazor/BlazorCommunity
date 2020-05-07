window.getScrollTop = function (el) {
    return el.scrollTop || 0;
};

window.getscrollHeight = function (el) {
    return el.scrollHeight || 0;
};


window.scroll = function (el, top) {
    el.scrollTop = top;
};
window.isScrollToBottom = function (el) {
    var scrollTop = el.scrollTop || 0;
    var scrollHeight = el.scrollHeight || 0;
    var clientHeight = el.clientHeight | 0;

    var res = scrollTop + clientHeight === scrollHeight;

    return { isScrollToBottom: res, ScrollHeight: scrollHeight }
}

window.isScrollToTop = function (el) {
    var scrollTop = el.scrollTop || 0;
    var bodyScrollTop = document.body.scrollTop || 0;

    return scrollTop == 0 && bodyScrollTop == 0;
}
//const ScrollTop = (el,number = 0, time) => {
//    if (!time) {
//        document.body.scrollTop = el.scrollTop = number;
//        return number;
//    }
//    const spacingTime = 20; // 设置循环的间隔时间  值越小消耗性能越高
//    let spacingInex = time / spacingTime; // 计算循环的次数
//    let nowTop = document.body.scrollTop + el.scrollTop; // 获取当前滚动条位置
//    let everTop = (number - nowTop) / spacingInex; // 计算每次滑动的距离
//    let scrollTimer = setInterval(() => {
//        if (spacingInex > 0) {
//            spacingInex--;
//            ScrollTop(el,nowTop += everTop);
//        } else {
//            clearInterval(scrollTimer); // 清除计时器
//        }
//    }, spacingTime);
//};