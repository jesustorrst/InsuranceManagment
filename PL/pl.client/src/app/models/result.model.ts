export interface Result<T> {
  correct: boolean;
  errorMessage: string;
  object?: T;
  objects?: T[];    
  ex?: any;
}
