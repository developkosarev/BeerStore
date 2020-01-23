export const currency = value => {
  return parseFloat(value).toFixed(2) + "руб.";
};

export const date = value => {
  const date = new Date(value);
  return date.toLocaleString('ru-RU');

  // const dd = (date.getDate() < 10 ? "0" : "") + date.getDate();
  // const MM = (date.getMonth() + 1 < 10 ? "0" : "") + (date.getMonth() + 1);
  // const yyyy = date.getFullYear();
  //
  // return dd + "." + MM + "." + yyyy;
  //
  // var options = {
  //   era: 'long',
  //   year: 'numeric',
  //   month: 'long',
  //   day: 'numeric',
  //   weekday: 'long',
  //   timezone: 'UTC',
  //   hour: 'numeric',
  //   minute: 'numeric',
  //   second: 'numeric'
  // };
  //
  // return date.toLocaleString("ru", options)
};
