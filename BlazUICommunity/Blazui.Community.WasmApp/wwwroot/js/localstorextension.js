Storage.prototype.setExpire = (key, value) => {
    let obj = {
        data: value,
        time: Date.now(),
        expire: 5000
    };
    //localStorage 设置的值不能为对象,转为json字符串
    console.log("localStorage");
    localStorage.setItem(key, JSON.stringify(obj));
}

Storage.prototype.getExpire = key => {
    let val = localStorage.getItem(key);
    if (!val) {
        return val;
    }
    val = JSON.parse(val);
    if (Date.now() - val.time > val.expire) {
        localStorage.removeItem(key);
        return null;
    }
    return val.data;
}