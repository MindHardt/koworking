import { clsx, type ClassValue } from 'clsx'
import { twMerge } from 'tailwind-merge'

/* Merges multiple tailwind classes together */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}
