export interface SnippetGroup {
  id: string;
  name: string;
  description: string;
  author: string;
  snippetCount: number;
  tags: string[];
  updatedAt: Date;
}