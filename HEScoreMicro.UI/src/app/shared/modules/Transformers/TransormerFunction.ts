export function arrayToObjectTransformer(ele: any, name: string) {
  if (!Array.isArray(ele?.[name])) return;
  let opt = [...ele?.[name]];
  if (ele && ele?.[name]) {
    delete ele?.[name];
  }
  ele[name] = {};
  for (let obj of opt) {
    for (let [key, value] of Object.entries(obj)) {
      if (key !== 'options') {
        ele[name][key] = value;
      }
    }
  }
}

export function removeNullIdProperties(obj: any): any {
  if (Array.isArray(obj)) {
    return obj.map(removeNullIdProperties).filter(item => item !== null);
  } else if (obj !== null && typeof obj === 'object') {
    const newObj: any = {};
    for (const key in obj) {
      if (obj[key] && typeof obj[key] === 'object') {
        obj[key] = removeNullIdProperties(obj[key]);
      }
      if (!(key === 'id' && obj[key] === null)) {
        newObj[key] = obj[key];
      }
    }
    return newObj;
  }
  return obj;
}

export function removeAllIdProperties(obj: any): any {
  if (Array.isArray(obj)) {
    return obj.map(removeAllIdProperties); // Recursively apply for arrays
  } else if (obj !== null && typeof obj === 'object') {
    const newObj: any = {};
    for (const key in obj) {
      if (obj[key] && typeof obj[key] === 'object') {
        obj[key] = removeAllIdProperties(obj[key]); // Recursively clean nested objects
      }
      if (!/id$/i.test(key)) { // Remove ALL properties ending with "id" or "Id"
        newObj[key] = obj[key];
      }
    }
    return newObj;
  }
  return obj;
}

export function getDirection(index: number | null | undefined): string {
  switch (index) {
    case 0:
      return "Front"
    case 1:
      return "Back"
    case 2:
      return "Right"
    case 3:
      return "Left"
  }
  return "";
}
