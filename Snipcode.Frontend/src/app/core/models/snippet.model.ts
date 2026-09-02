import { Technology } from "./technology.enum";

export interface Snippet {
  id: string; // Guid
  title: string;
  description: string | null; // string?
  technology: Technology;
  codeContent: string;
  isPublic: boolean;
  createdAt: string; // DateTime
  updatedAt: string; // DateTime
  authorId: string; // Guid
  authorUsername: string;
  groupId: string | null; // Guid?
  tags: string[];
}