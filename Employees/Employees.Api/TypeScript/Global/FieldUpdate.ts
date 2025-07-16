export function assignField<T extends object, K extends keyof T>(target: T, key: K, value: T[K])
{
    target[key] = value;    
}